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
        private const string LogPath = @"C:\ProgramData\ConsoleContainer\FrontEnd\Logs\test.log";

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                System.IO.File.AppendAllLines(LogPath, ["Starting application!!!!"]);

                await AppHost.StartAsync();

                appManager = AppHost.Services.GetRequiredService<AppManager>();
                await appManager.StartAsync(cancellationTokenSource.Token);

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                var loggedException = ex;
                while (loggedException is not null)
                {
                    System.IO.File.AppendAllLines(LogPath, [loggedException.Message]);
                    loggedException = loggedException.InnerException;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            OnExit().Wait();

            base.OnExit(e);
        }

        private async Task OnExit()
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
        }
    }
}
