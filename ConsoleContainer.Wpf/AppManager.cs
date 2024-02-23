using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    internal class AppManager(
        ILogger<AppManager> logger,
        MainWindow mainWindow,
        IProcessHubClient processHubClient,
        IEventAggregator eventAggregator
    )
    {
        private readonly List<Task> cancellableTasks = new();

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                PlatformProvider.Current = new WindowsPlatformProvider();

                await StartProcessHubClientAsync(cancellationToken);

                mainWindow.Show();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error running application.");

                MessageBox.Show("Could not start application.");

                throw;
            }
        }

        public async Task StopAsync()
        {
            foreach (var task in cancellableTasks)
            {
                try { await task.ConfigureAwait(false); } catch { }
            }

            processHubClient?.Dispose();
        }

        private async Task StartProcessHubClientAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Process Hub Client");
            await processHubClient.StartAsync().ConfigureAwait(false);

            var stream = processHubClient.GetProcessOutputDataStream(cancellationToken);
            cancellableTasks.Add(HandleOutputDataAsync(stream, eventAggregator));
        }

        private async Task HandleOutputDataAsync(IAsyncEnumerable<ProcessOutputDataDto> stream, IEventAggregator eventAggregator)
        {
            logger.LogInformation("Listenning on ProcessOutputDataStream");
            await foreach (var data in stream)
            {
                await eventAggregator.PublishOnCurrentThreadAsync(new ProcessOutputDataReceivedEvent(data));
            }
        }
    }
}
