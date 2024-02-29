using ConsoleContainer.Contracts;
using ConsoleContainer.Kernel.Validation;
using ConsoleContainer.ProcessManagement;
using ConsoleContainer.ProcessManagement.Events;
using ConsoleContainer.WorkerService.Services;
using Newtonsoft.Json;

namespace ConsoleContainer.WorkerService
{
    public sealed class ProcessWorker(
        ILogger<ProcessWorker> logger,
        IProcessHubSubscription processHubSubscription,
        IOutputDataChannelWriter outputDataChannelWriter,
        IProcessManager<ProcessKey> processManager,
        IProcessGroupService processGroupService
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            processManager.ProcessAdded += ProcessManager_ProcessAdded;
            processManager.ProcessRemoved += ProcessManager_ProcessRemoved;

            await InitializeProcessesAsync(stoppingToken);

            await Task.Run(() =>
            {
                WaitHandle.WaitAny(new[] { stoppingToken.WaitHandle });
            });

            processManager.ProcessAdded -= ProcessManager_ProcessAdded;
            processManager.ProcessRemoved -= ProcessManager_ProcessRemoved;
        }

        private async Task InitializeProcessesAsync(CancellationToken stoppingToken)
        {
            var groups = await processGroupService.GetProcessGroupsAsync(stoppingToken);
            var processDetails = from g in groups
                                 from p in g.Processes
                                 select new
                                 {
                                     Key = new ProcessKey(g.ProcessGroupId, p.ProcessLocator),
                                     Details = new ProcessDetails(
                                         p.FilePath.Required(),
                                         p.Arguments,
                                         p.WorkingDirectory
                                     ),
                                     p.AutoStart
                                 };

            var tasks = processDetails.Select(async d =>
            {
                var process = await processManager.CreateProcessAsync(d.Key, d.Details);
                if (d.AutoStart)
                {
                    await process.StartProcessAsync();
                }
            });

            await Task.WhenAll(tasks);
        }

        private void ProcessManager_ProcessAdded(object? sender, ProcessAddedEventArgs<ProcessKey> e)
        {
            var process = e.Process;
            process.StateChanged += Process_StateChanged;
            process.OutputDataReceived += Process_OutputDataReceived;
        }

        private void ProcessManager_ProcessRemoved(object? sender, ProcessRemovedEventArgs<ProcessKey> e)
        {
            var process = e.Process;
            process.StateChanged -= Process_StateChanged;
            process.OutputDataReceived -= Process_OutputDataReceived;
        }

        private void Process_StateChanged(object? sender, ProcessStateChangedEventArgs<ProcessKey> e)
        {
            logger.LogInformation($"Sending state changed to process {e.ProcessKey}: {e.State}");
            _ = processHubSubscription.ProcessStateUpdatedAsync(e.ProcessKey.ProcessGroupId, e.ProcessKey.ProcessLocator, (Contracts.ProcessState)e.State, e.ProcessId);
        }

        private void Process_OutputDataReceived(object? sender, ProcessOutputDataEventArgs<ProcessKey> e)
        {
            logger.LogInformation($"Sending output data to process {e.ProcessKey}: {JsonConvert.SerializeObject(e.Data)}");
            _ = outputDataChannelWriter.WriteOutputDataAsync(new ProcessOutputDataDto()
            {
                ProcessGroupId = e.ProcessKey.ProcessGroupId,
                ProcessLocator = e.ProcessKey.ProcessLocator,
                Data = e.Data.Data,
                IsProcessError = e.Data.IsProcessError
            });
        }
    }
}
