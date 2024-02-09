using Microsoft.Extensions.DependencyInjection;

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
            services.AddWpf();

            return services.BuildServiceProvider();
        }
    }
}
