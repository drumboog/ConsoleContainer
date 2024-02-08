using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Hubs;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IEventAggregator EventAggregator => ServiceProvider.GetRequiredService<IEventAggregator>();

        private static Lazy<IServiceProvider> serviceProvider = new Lazy<IServiceProvider>(CreateServiceProvider);
        public static IServiceProvider ServiceProvider => serviceProvider.Value;

        private IProcessHubClient ProcessHubClient => ServiceProvider.GetRequiredService<IProcessHubClient>();

        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessHubClient.StartAsync().Wait();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ProcessHubClient.Dispose();

            base.OnExit(e);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddWpf();

            return services.BuildServiceProvider();
        }
    }
}
