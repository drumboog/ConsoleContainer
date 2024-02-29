using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.Services;
using ConsoleContainer.Wpf.ViewModels.Dialogs;
using ConsoleContainer.Wpf.ViewModels.Factories;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessContainerVM : ViewModel,
        IHandle<EditProcessEvent>,
        IHandle<DeleteProcessEvent>,
        IHandle<ProcessGroupCreatedEvent>,
        IHandle<ProcessGroupUpdatedEvent>,
        IHandle<ProcessGroupDeletedEvent>,
        IHandle<ProcessCreatedEvent>,
        IHandle<ProcessUpdatedEvent>,
        IHandle<ProcessStateUpdatedEvent>,
        IHandle<ProcessOutputDataReceivedEvent>,
        IHandle<ProcessDeletedEvent>
    {
        private readonly IDialogService dialogService;
        private readonly IWorkerServiceClient workerServiceClient;
        private readonly IProcessGroupVmFactory processGroupVmFactory;
        private readonly IProcessVmFactory processVmFactory;

        public ObservableCollection<ProcessGroupVM> ProcessGroups { get; } = new();

        public Control? Dialog
        {
            get => GetProperty<Control>();
            set => SetProperty(value);
        }

        public ProcessContainerVM(
            IDialogService dialogService,
            IWorkerServiceClient workerServiceClient,
            IProcessGroupVmFactory processGroupVmFactory,
            IProcessVmFactory processVmFactory,
            IEventAggregator eventAggregator
        )
        {
            this.dialogService = dialogService;
            this.workerServiceClient = workerServiceClient;
            this.processGroupVmFactory = processGroupVmFactory;
            this.processVmFactory = processVmFactory;

            eventAggregator.SubscribeOnUIThread(this);
        }

        public void AddProcessGroup(ProcessGroupVM processGroup)
        {
            ProcessGroups.Add(processGroup);
        }

        public async Task CreateProcessGroupAsync()
        {
            var newGroup = await dialogService.CreateProcessGroupAsync();
            if (newGroup is null)
            {
                return;
            }

            await workerServiceClient.CreateProcessGroupAsync(new ProcessGroupDto() { ProcessGroupId = Guid.NewGuid(), GroupName = newGroup.GroupName });
        }

        public async Task EditProcessGroupAsync(ProcessGroupVM processGroup)
        {
            var vm = new EditProcessGroupVM() { GroupName = processGroup.GroupName };
            if (!await dialogService.EditProcessGroupAsync(vm))
            {
                return;
            }

            await workerServiceClient.UpdateProcessGroupAsync(processGroup.ProcessGroupId, new ProcessGroupUpdateDto() { GroupName = vm.GroupName });
        }

        public async Task DeleteProcessGroupAsync(ProcessGroupVM processGroup)
        {
            var result = await dialogService.ShowConfirmationDialogAsync(new ConfirmationDialogVM("Delete Process Group?", $"Are you sure you want to delete the process group: {processGroup.GroupName}?"));
            if (!result)
            {
                return;
            }

            await workerServiceClient.DeleteProcessGroupAsync(processGroup.ProcessGroupId);
        }

        public async Task CreateProcessAsync(ProcessGroupVM processGroup)
        {
            var result = await dialogService.CreateProcessAsync();
            if (result is null)
            {
                return;
            }

            await workerServiceClient.CreateProcessAsync(
                processGroup.ProcessGroupId,
                new ProcessInformationDto()
                {
                    ProcessLocator = Guid.NewGuid(),
                    ProcessName = result.ProcessName,
                    FilePath = result.FilePath,
                    Arguments = result.Arguments,
                    WorkingDirectory = result.WorkingDirectory
                });
        }

        public async Task RefreshProcessesAsync()
        {
            var runningProcesses = ProcessGroups.SelectMany(x => x.Processes).Where(x => x.IsRunning);
            if (runningProcesses.Any())
            {
                throw new Exception("Cannot refresh when processes are running");
            }

            var savedProcessGroups = await workerServiceClient.GetProcessGroupsAsync(CancellationToken.None);

            var newProcessGroups = new List<ProcessGroupVM>();
            foreach (var processGroup in savedProcessGroups)
            {
                newProcessGroups.Add(CreateProcessGroup(processGroup));
            }

            ProcessGroups.Clear();
            newProcessGroups.ForEach(x => ProcessGroups.Add(x));
        }

        private ProcessGroupVM CreateProcessGroup(ProcessGroupSummaryDto processGroup)
        {
            var vm = processGroupVmFactory.Create(processGroup.ProcessGroupId, processGroup.GroupName);
            foreach(var p in processGroup.Processes)
            {
                var newProcess = processVmFactory.Create(processGroup.ProcessGroupId, p.ProcessLocator, p.ProcessId, p.ProcessName ?? string.Empty, p.FilePath ?? string.Empty, p.Arguments, p.WorkingDirectory, p.AutoStart, p.State);
                vm.Processes.Add(newProcess);
            }

            return vm;
        }

        async Task IHandle<EditProcessEvent>.HandleAsync(EditProcessEvent message, CancellationToken cancellationToken)
        {
            var process = message.Process;
            var processGroup = ProcessGroups.FirstOrDefault(g => g.Processes.Any(p => p.ProcessLocator == process.ProcessLocator));
            if (processGroup is null)
            {
                return;
            }

            var vm = new EditProcessVM()
            {
                ProcessName = process.ProcessName,
                FilePath = process.FilePath,
                Arguments = process.Arguments,
                WorkingDirectory = process.WorkingDirectory,
                AutoStart = process.AutoStart
            };

            if (!await dialogService.EditProcessAsync(vm))
            {
                return;
            }

            await workerServiceClient.UpdateProcessAsync(
                processGroup.ProcessGroupId,
                process.ProcessLocator,
                new ProcessInformationUpdateDto()
                {
                    ProcessName = vm.ProcessName,
                    FilePath = vm.FilePath,
                    Arguments = vm.Arguments,
                    WorkingDirectory = vm.WorkingDirectory,
                    AutoStart = vm.AutoStart
                });
        }

        async Task IHandle<DeleteProcessEvent>.HandleAsync(DeleteProcessEvent message, CancellationToken cancellationToken)
        {
            var process = message.Process;
            var result = await dialogService.ShowConfirmationDialogAsync(new ConfirmationDialogVM("Delete Process?", $"Are you sure you want to delete the process: {process.ProcessName}?"));
            if (!result)
            {
                return;
            }

            await workerServiceClient.DeleteProcessAsync(process.ProcessGroupId, process.ProcessLocator);
        }

        Task IHandle<ProcessGroupCreatedEvent>.HandleAsync(ProcessGroupCreatedEvent message, CancellationToken cancellationToken)
        {
            var group = message.ProcessGroup;
            ProcessGroups.Add(processGroupVmFactory.Create(group.ProcessGroupId, group.GroupName));
            return Task.CompletedTask;
        }

        Task IHandle<ProcessGroupUpdatedEvent>.HandleAsync(ProcessGroupUpdatedEvent message, CancellationToken cancellationToken)
        {
            var updatedGroup = message.ProcessGroup;
            var group = ProcessGroups.FirstOrDefault(p => p.ProcessGroupId == updatedGroup.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            group.Update(updatedGroup.GroupName);

            return Task.CompletedTask;
        }

        Task IHandle<ProcessGroupDeletedEvent>.HandleAsync(ProcessGroupDeletedEvent message, CancellationToken cancellationToken)
        {
            var group = ProcessGroups.FirstOrDefault(p => p.ProcessGroupId == message.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            ProcessGroups.Remove(group);

            return Task.CompletedTask;
        }

        Task IHandle<ProcessCreatedEvent>.HandleAsync(ProcessCreatedEvent message, CancellationToken cancellationToken)
        {
            var group = ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == message.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            var pi = message.ProcessInformation;
            var process = processVmFactory.Create(message.ProcessGroupId, pi.ProcessLocator, pi.ProcessId, pi.ProcessName, pi.FilePath, pi.Arguments, pi.WorkingDirectory, pi.AutoStart, pi.State);
            group.AddProcess(process);

            return Task.CompletedTask;
        }

        Task IHandle<ProcessUpdatedEvent>.HandleAsync(ProcessUpdatedEvent message, CancellationToken cancellationToken)
        {
            var group = ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == message.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            var pi = message.ProcessInformation;
            var process = group.Processes.FirstOrDefault(p => p.ProcessLocator == pi.ProcessLocator);
            if (process is null)
            {
                return Task.CompletedTask;
            }

            process.Update(pi.ProcessName, pi.FilePath, pi.Arguments, pi.WorkingDirectory, pi.AutoStart);

            return Task.CompletedTask;
        }

        Task IHandle<ProcessStateUpdatedEvent>.HandleAsync(ProcessStateUpdatedEvent message, CancellationToken cancellationToken)
        {
            var group = ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == message.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            var process = group.Processes.FirstOrDefault(p => p.ProcessLocator == message.ProcessLocator);
            if (process is null)
            {
                return Task.CompletedTask;
            }

            process.UpdateState(message.State, message.ProcessId);

            return Task.CompletedTask;
        }

        Task IHandle<ProcessOutputDataReceivedEvent>.HandleAsync(ProcessOutputDataReceivedEvent message, CancellationToken cancellationToken)
        {
            var data = message.Data;

            var group = ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == data.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            var process = group.Processes.FirstOrDefault(p => p.ProcessLocator == data.ProcessLocator);
            if (process is null)
            {
                return Task.CompletedTask;
            }

            process.Output.AddOutput(data.Data);

            return Task.CompletedTask;
        }

        Task IHandle<ProcessDeletedEvent>.HandleAsync(ProcessDeletedEvent message, CancellationToken cancellationToken)
        {
            var group = ProcessGroups.FirstOrDefault(g => g.ProcessGroupId == message.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }

            group.DeleteProcess(message.ProcessLocator);

            return Task.CompletedTask;
        }
    }
}
