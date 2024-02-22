using ConsoleContainer.Wpf.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NReco.Logging.File;
using System.IO;

namespace ConsoleContainer.Wpf
{
    internal class ServiceLocator
    {
        private static Lazy<IServiceProvider> serviceProvider = new Lazy<IServiceProvider>(CreateServiceProvider);
        public static IServiceProvider ServiceProvider => serviceProvider.Value;

        public static T GetService<T>()
            where T : notnull
        {
            return serviceProvider.Value.GetRequiredService<T>();
        }

        private static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            var config = BuildConfiguration();

            var applicationSettings = config.GetRequiredValue<ApplicationSettings>("ApplicationSettings");
            var applicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), applicationSettings.ApplicationDataDirectoryName);
            var loggingRootPath = Path.Join(applicationDataDirectory, "Logs");
            var logFile = Path.Join(loggingRootPath, "wpf.log");

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFile(logFile, options =>
                {
                    options.Append = true;
                    options.MaxRollingFiles = 100;
                    options.FileSizeLimitBytes = 1024 * 1024 * 10; // 10MB
                });
            });

            services.AddWpf();

            return services.BuildServiceProvider();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            var environment = Environment.GetEnvironmentVariable("ASPCORE_ENVIRONMENT");
            if (environment is not null)
            {
                builder.AddJsonFile($"appsettings.{environment}.json", true, true);
            }

            return builder.Build();
        }
    }
}
