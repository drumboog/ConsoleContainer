using Microsoft.Extensions.DependencyInjection;
using NReco.Logging.File;
using System.Configuration;
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

            var applicationDataDirectoryName = ConfigurationManager.AppSettings["ApplicationDataDirectoryName"] ?? throw new Exception("Application Data Directory Name not configured");
            var applicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), applicationDataDirectoryName);
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
    }
}
