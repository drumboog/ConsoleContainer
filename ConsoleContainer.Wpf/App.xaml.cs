using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            PlatformProvider.Current = new WindowsPlatformProvider();

            StartProcessHubClient().Wait();

            base.OnStartup(e);
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
            processHubClient = ServiceLocator.GetService<IProcessHubClient>();
            await processHubClient.StartAsync().ConfigureAwait(false);

            var eventAggregator = ServiceLocator.GetService<IEventAggregator>();
            var stream = processHubClient.GetProcessOutputDataStream(cancellationTokenSource.Token);
            cancellableTasks.Add(HandleOutputDataAsync(stream, eventAggregator));
        }

        private async Task HandleOutputDataAsync(IAsyncEnumerable<ProcessOutputDataDto> stream, IEventAggregator eventAggregator)
        {
            await foreach (var data in stream)
            {
                await eventAggregator.PublishOnCurrentThreadAsync(new ProcessOutputDataReceivedEvent(data));
            }
        }
    }
}
