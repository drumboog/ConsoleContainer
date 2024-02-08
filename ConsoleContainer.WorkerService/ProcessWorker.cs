using ConsoleContainer.Contracts;
using ConsoleContainer.ProcessManagement;
using ConsoleContainer.ProcessManagement.Events;

namespace ConsoleContainer.WorkerService
{
    public sealed class ProcessWorker(
        ILogger<ProcessWorker> logger,
        IProcessHubSubscription processHubSubscription,
        IProcessManager processManager
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            processManager.ProcessAdded += ProcessManager_ProcessAdded;
            processManager.ProcessRemoved += ProcessManager_ProcessRemoved;

            await Task.Run(() =>
            {
                WaitHandle.WaitAny(new[] { stoppingToken.WaitHandle });
            });

            processManager.ProcessAdded -= ProcessManager_ProcessAdded;
            processManager.ProcessRemoved -= ProcessManager_ProcessRemoved;
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
