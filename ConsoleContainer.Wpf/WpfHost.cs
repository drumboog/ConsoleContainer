using ConsoleContainer.Wpf.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using System.IO;

namespace ConsoleContainer.Wpf
{
    internal static class WpfHost
    {
        public static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureServices(services);
                });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var config = BuildConfiguration();

            var applicationSettings = config.GetRequiredValue<ApplicationSettings>("ApplicationSettings");
            var applicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), applicationSettings.ApplicationDataDirectoryName);
            var loggingRootPath = Path.Join(applicationDataDirectory, "Logs");
            var logFile = Path.Join(loggingRootPath, "wpf.log");

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddFile(logFile, options =>
                {
                    options.Append = true;
                    options.MaxRollingFiles = 100;
                    options.FileSizeLimitBytes = 1024 * 1024 * 10; // 10MB
                });
            });

            services.AddSingleton<AppManager>();

            services.AddWpf(applicationSettings);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            var environment = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT") ?? "Production";
            builder.AddJsonFile($"appsettings.{environment}.json", true, true);

            return builder.Build();
        }
    }
}
