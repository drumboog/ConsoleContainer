using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Hubs;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IEventAggregator EventAggregator { get; } = new EventAggregator();

        private ProcessHubClient? processHubClient;

        protected override void OnStartup(StartupEventArgs e)
        {
            _ = StartProcessHubClientAsync();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                processHubClient?.CloseHubAsync()?.Wait();
            }
            catch { }

            base.OnExit(e);
        }

        private async Task StartProcessHubClientAsync()
        {
            try
            {
                processHubClient = new ProcessHubClient("https://localhost:7276/signalr/Process", "Process");
                processHubClient.CreateSubscription(new ProcessHubSubscription(EventAggregator));
                await processHubClient.StartAsync();
            }
            catch { }
        }
    }
}
