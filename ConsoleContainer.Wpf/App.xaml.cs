using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessHubClient? processHubClient;

        protected override void OnStartup(StartupEventArgs e)
        {
            PlatformProvider.Current = new WindowsPlatformProvider();

            processHubClient = ServiceLocator.GetService<IProcessHubClient>();
            processHubClient.StartAsync().Wait();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            processHubClient?.Dispose();

            base.OnExit(e);
        }
    }
}
