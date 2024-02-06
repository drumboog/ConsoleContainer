using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.Eventing
{
    public static class ServiceCollectionEventingExtensions
    {
        public static IServiceCollection AddEventing(this IServiceCollection services)
        {
            services.AddSingleton<IEventAggregator, EventAggregator>();
            return services;
        }
    }
}
