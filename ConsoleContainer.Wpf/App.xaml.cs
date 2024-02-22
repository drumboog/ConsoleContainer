using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly List<Task> cancellableTasks = new();
        private IProcessHubClient? processHubClient;

        private readonly Lazy<ILogger<App>> lazyLogger = new Lazy<ILogger<App>>(() => ServiceLocator.GetService<ILogger<App>>());
        private ILogger<App> Logger => lazyLogger.Value;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                PlatformProvider.Current = new WindowsPlatformProvider();

                StartProcessHubClient().Wait();

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error running application.");

                MessageBox.Show("Could not start application.");

                throw;
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            cancellationTokenSource.Cancel(false);
            foreach (var task in cancellableTasks)
            {
                try { await task.ConfigureAwait(false); } catch { }
            }

            processHubClient?.Dispose();

            base.OnExit(e);
        }

        private async Task StartProcessHubClient()
        {
            Logger.LogInformation("Getting Process Hub Client");

            processHubClient = ServiceLocator.GetService<IProcessHubClient>();

            Logger.LogInformation("Starting Process Hub Client");
            await processHubClient.StartAsync().ConfigureAwait(false);

            var eventAggregator = ServiceLocator.GetService<IEventAggregator>();
            var stream = processHubClient.GetProcessOutputDataStream(cancellationTokenSource.Token);
            cancellableTasks.Add(HandleOutputDataAsync(stream, eventAggregator));
        }

        private async Task HandleOutputDataAsync(IAsyncEnumerable<ProcessOutputDataDto> stream, IEventAggregator eventAggregator)
        {
            Logger.LogInformation("Listenning on ProcessOutputDataStream");
            await foreach (var data in stream)
            {
                await eventAggregator.PublishOnCurrentThreadAsync(new ProcessOutputDataReceivedEvent(data));
            }
        }
    }
}
