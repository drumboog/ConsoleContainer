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
            try
            {
                var process = e.Process;
                processHubSubscription.ProcessAdded(new ProcessDto()
                {
                    ProcessLocator = process.ProcessLocator,
                    ProcessId = process.ProcessId,
                    FilePath = process.ProcessDetails.FilePath,
                    Arguments = process.ProcessDetails.Arguments,
                    WorkingDirectory = process.ProcessDetails.WorkingDirectory
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception was thrown while notifying of a added process: {0}", e.Process.ProcessLocator);
            }
        }

        private void ProcessManager_ProcessRemoved(object? sender, ProcessRemovedEventArgs e)
        {
            try
            {
                var process = e.Process;
                processHubSubscription.ProcessRemoved(new ProcessDto()
                {
                    ProcessLocator = process.ProcessLocator,
                    ProcessId = process.ProcessId,
                    FilePath = process.ProcessDetails.FilePath,
                    Arguments = process.ProcessDetails.Arguments,
                    WorkingDirectory = process.ProcessDetails.WorkingDirectory
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception was thrown while notifying of a removed process: {0}", e.Process.ProcessLocator);
            }
        }
    }
}
