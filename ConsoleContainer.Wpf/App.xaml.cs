using ConsoleContainer.Contracts;
using ConsoleContainer.Eventing;
using ConsoleContainer.WorkerService.Client;
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
        public static IProcessHub ProcessHub { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            _ = StartProcessHubClientAsync();

            base.OnStartup(e);
        }

        private async Task StartProcessHubClientAsync()
        {
            try
            {
                var processHubClient = new ProcessHubClient("https://localhost:7276/signalr/Process", "Process");
                ProcessHub = processHubClient;
                processHubClient.CreateSubscription(new ProcessHubSubscription(EventAggregator));
                await processHubClient.StartAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
