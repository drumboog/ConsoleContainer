using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();

        private readonly static Lazy<IHost> lazyHost = new Lazy<IHost>(() => WpfHost.CreateDefaultBuilder().Build());
        private static IHost AppHost => lazyHost.Value;

        private AppManager? appManager;

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            appManager = AppHost.Services.GetRequiredService<AppManager>();
            await appManager.StartAsync(cancellationTokenSource.Token);

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (AppHost)
            {
                cancellationTokenSource.Cancel(false);

                var task = appManager?.StopAsync();
                if (task is not null)
                {
                    await task;
                }

                await AppHost.StopAsync();
            }

            base.OnExit(e);
        }
    }
}
