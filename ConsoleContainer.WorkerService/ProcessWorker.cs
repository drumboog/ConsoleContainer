using ConsoleContainer.Contracts;
using ConsoleContainer.Kernel.Validation;
using ConsoleContainer.ProcessManagement;
using ConsoleContainer.ProcessManagement.Events;
using ConsoleContainer.WorkerService.Services;

namespace ConsoleContainer.WorkerService
{
    public sealed class ProcessWorker(
        ILogger<ProcessWorker> logger,
        IProcessHubSubscription processHubSubscription,
        IProcessManager processManager,
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
            var processes = groups.SelectMany(g => g.Processes);

            var processDetails = processes.Select(p => new ProcessDetails(p.ProcessLocator.Required(), p.FilePath.Required(), p.Arguments, p.WorkingDirectory));

            var tasks = processDetails.Select(d => processManager.CreateProcessAsync(d));

            await Task.WhenAll(tasks);
        }

        private void ProcessManager_ProcessAdded(object? sender, ProcessAddedEventArgs e)
        {
            var process = e.Process;
            process.OutputDataReceived += Process_OutputDataReceived;
        }

        private void ProcessManager_ProcessRemoved(object? sender, ProcessRemovedEventArgs e)
        {
            var process = e.Process;
            process.OutputDataReceived -= Process_OutputDataReceived;
        }

        private void Process_OutputDataReceived(object? sender, ProcessOutputDataEventArgs e)
        {
            logger.LogInformation($"Sending output data to process {e.ProcessDetails.ProcessLocator}: {e.Data}");
            _ = processHubSubscription.ProcessOutputDataReceivedAsync(e.ProcessDetails.ProcessLocator, new ProcessOutputDataDto()
            {
                Data = e.Data.Data,
                IsProcessError = e.Data.IsProcessError
            });
        }
    }
}
