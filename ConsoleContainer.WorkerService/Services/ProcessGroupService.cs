using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;
using ConsoleContainer.Kernel.Validation;
using ConsoleContainer.ProcessManagement;
using ConsoleContainer.Repositories;
using ConsoleContainer.WorkerService.Mappers;

namespace ConsoleContainer.WorkerService.Services
{
    public class ProcessGroupService(
        IProcessGroupMapper processGroupMapper,
        IProcessMapper processMapper,
        IProcessManager<ProcessKey> processManager,
        IProcessGroupCollectionRepository processGroupCollectionRepository,
        IProcessHubSubscription processHubSubscription
    ) : IProcessGroupService
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private ProcessGroupCollection? collection;

        public async Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken)
        {
            var groups = await GetProcessGroupCollectionAsync(cancellationToken);
            return groups.ProcessGroups.Select(g => processGroupMapper.MapSummary(g));
        }

        public Task CreateProcessGroupAsync(ProcessGroupDto processGroup)
        {
            return UpdateAsync(async context =>
            {
                var collection = await GetProcessGroupCollectionAsync(CancellationToken.None);
                var newGroup = collection.AddGroup(processGroup.ProcessGroupId, processGroup.GroupName);

                context.QueuePostProcessingAction(() => processHubSubscription.ProcessGroupCreatedAsync(processGroupMapper.Map(newGroup)));
            });
        }

        public Task<bool> UpdateProcessGroupAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup)
        {
            return UpdateAsync(async context =>
            {
                var collection = await GetProcessGroupCollectionAsync(CancellationToken.None);
                var group = collection.ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == processGroupId);
                if (group is null)
                {
                    return false;
                }

                group.Update(processGroup.GroupName);

                context.QueuePostProcessingAction(() => processHubSubscription.ProcessGroupUpdatedAsync(processGroupMapper.Map(group)));

                return true;
            });
        }

        public Task<bool> DeleteProcessGroupAsync(Guid processGroupId)
        {
            return UpdateAsync(async context =>
            {
                var collection = await GetProcessGroupCollectionAsync(CancellationToken.None);

                var group = collection[processGroupId];
                if (group is null)
                {
                    return false;
                }

                var deleteProcessTasks = group.Processes.Select(p => processManager.DeleteProcessAsync(new ProcessKey(processGroupId, p.ProcessLocator)));
                await Task.WhenAll(deleteProcessTasks);

                var result = collection.DeleteGroup(processGroupId);

                if (result)
                {
                    context.QueuePostProcessingAction(() => processHubSubscription.ProcessGroupDeletedAsync(processGroupId));
                }

                return result;
            });
        }

        public Task<bool> CreateProcessAsync(Guid processGroupId, ProcessInformationDto processInformation)
        {
            return UpdateAsync(async context =>
            {
                var collection = await GetProcessGroupCollectionAsync(CancellationToken.None);
                var group = collection.ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == processGroupId);
                if (group is null)
                {
                    return false;
                }

                var processExists = group.Processes.Any(p => p.ProcessLocator == processInformation.ProcessLocator);
                if (processExists)
                {
                    throw new Exception($"Process already exists with locator {processInformation.ProcessLocator}");
                }

                await processManager.CreateProcessAsync(
                    new ProcessKey(processGroupId, processInformation.ProcessLocator),
                    new ProcessDetails(
                        processInformation.FilePath.Required(),
                        processInformation.Arguments,
                        processInformation.WorkingDirectory));

                var newProcess = group.AddProcess(processInformation.ProcessLocator, processInformation);

                context.QueuePostProcessingAction(() => processHubSubscription.ProcessCreatedAsync(processGroupId, processMapper.Map(group.ProcessGroupId, newProcess)));

                return true;
            });
        }

        public Task<bool> UpdateProcessAsync(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto processInformation)
        {
            return UpdateAsync(async context =>
            {
                var collection = await GetProcessGroupCollectionAsync(CancellationToken.None);
                var group = collection.ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == processGroupId);
                if (group is null)
                {
                    return false;
                }

                EnsureProcessIsStopped(processGroupId, processLocator);

                var key = new ProcessKey(processGroupId, processLocator);
                var currentProcess = processManager.GetProcess(key);
                currentProcess?.UpdateProcessDetails(new ProcessDetails(processInformation.FilePath.Required(), processInformation.Arguments, processInformation.WorkingDirectory));

                var newProcess = group.ReplaceProcess(processLocator, processInformation);
                if (newProcess is null)
                {
                    return false;
                }

                context.QueuePostProcessingAction(() => processHubSubscription.ProcessUpdatedAsync(processGroupId, processMapper.Map(processGroupId, newProcess)));

                return true;
            });
        }

        public async Task<bool> StartProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var process = processManager.GetProcess(new ProcessKey(processGroupId, processLocator));
            if (process is null)
            {
                return false;
            }

            return await process.StartProcessAsync();
        }

        public async Task<bool> StopProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var process = processManager.GetProcess(new ProcessKey(processGroupId, processLocator));
            if (process is null)
            {
                return false;
            }

            return await process.StopProcessAsync();
        }

        public Task<bool> DeleteProcessAsync(Guid processGroupId, Guid processLocator)
        {
            return UpdateAsync(async context =>
            {
                var collection = await GetProcessGroupCollectionAsync(CancellationToken.None);
                var group = collection.ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == processGroupId);
                if (group is null)
                {
                    return false;
                }

                EnsureProcessIsStopped(processGroupId, processLocator);

                await processManager.DeleteProcessAsync(new ProcessKey(processGroupId, processLocator));

                var result = group.DeleteProcess(processLocator);

                context.QueuePostProcessingAction(() => processHubSubscription.ProcessDeletedAsync(processGroupId, processLocator));

                return result;
            });
        }

        private Task UpdateAsync(Func<UpdateContext, Task> action)
        {
            return UpdateAsync(async (context) =>
            {
                await action(context);
                return true;
            });
        }

        private Task<bool> UpdateAsync(Func<UpdateContext, Task<bool>> action)
        {
            return LockAsync(async () =>
            {
                var context = new UpdateContext();
                var result = await action(context);
                if (result)
                {
                    await SaveProcessGroupCollectionAsync();
                    await context.ExecutePostProcessingActionsAsync();
                }
                return result;
            },
            CancellationToken.None);
        }

        private async Task<T> LockAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return await action();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<ProcessGroupCollection> GetProcessGroupCollectionAsync(CancellationToken cancellationToken)
        {
            if (collection is null)
            {
                collection = await processGroupCollectionRepository.ReadAsync();
            }

            return collection;
        }

        private async Task SaveProcessGroupCollectionAsync()
        {
            if (collection is null)
            {
                return;
            }

            await processGroupCollectionRepository.SaveAsync(collection);
        }

        private void EnsureProcessIsStopped(Guid processGroupId, Guid processLocator)
        {
            if (IsProcessRunning(processGroupId, processLocator))
            {
                throw new Exception($"Cannot update running process: {processLocator}");
            }
        }

        private bool IsProcessRunning(Guid processGroupId, Guid processLocator)
        {
            var p = processManager.GetProcess(new ProcessKey(processGroupId, processLocator));
            if (p is null)
            {
                return false;
            }
            return p.State != ProcessManagement.ProcessState.Idle;
        }

        private class UpdateContext
        {
            private readonly List<Func<Task>> postProcessingActions = new();

            public void QueuePostProcessingAction(Func<Task> action)
            {
                postProcessingActions.Add(action);
            }

            public async Task ExecutePostProcessingActionsAsync()
            {
                var tasks = postProcessingActions.Select(x => x());
                await Task.WhenAll(tasks);
            }
        }
    }
}
