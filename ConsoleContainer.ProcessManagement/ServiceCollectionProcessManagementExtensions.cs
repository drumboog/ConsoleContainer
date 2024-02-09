using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.ProcessManagement
{
    public static class ServiceCollectionProcessManagementExtensions
    {
        public static IServiceCollection AddProcessManagement(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IProcessManager<>), typeof(ProcessManager<>));
            services.AddTransient(typeof(IProcessWrapperFactory<>), typeof(ProcessWrapperFactory<>));
            return services;
        }
    }
}
