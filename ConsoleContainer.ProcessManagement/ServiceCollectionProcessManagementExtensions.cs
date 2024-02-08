using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.ProcessManagement
{
    public static class ServiceCollectionProcessManagementExtensions
    {
        public static IServiceCollection AddProcessManagement(this IServiceCollection services)
        {
            services.AddSingleton<IProcessManager, ProcessManager>();
            services.AddTransient<IProcessWrapperFactory, ProcessWrapperFactory>();
            return services;
        }
    }
}
