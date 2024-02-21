using Microsoft.Extensions.DependencyInjection;
using NReco.Logging.File;
using System.IO;
using System.Reflection;

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

            var loggingRootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs");
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
