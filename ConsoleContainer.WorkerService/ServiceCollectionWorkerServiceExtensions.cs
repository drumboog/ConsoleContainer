using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.HubSubscriptions;
using ConsoleContainer.WorkerService.Services;

namespace ConsoleContainer.WorkerService
{
    public static class ServiceCollectionWorkerServiceExtensions
    {
        private static readonly string[] AutoRegistrationSuffixes = ["Mapper"];

        public static IServiceCollection AddWorkerService(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses(filter => filter.Where(t => ShouldAutoRegister(t)))
                    .AsMatchingInterface()
                    .WithTransientLifetime()
            );

            services.AddTransient<IProcessHubSubscription, ProcessHubSubscription>();
            services.AddSingleton<IProcessGroupService, ProcessGroupService>();
            services.AddSingleton<OutputDataChannelManager>();
            services.AddSingleton<IOutputDataChannelReader>(provider => provider.GetRequiredService<OutputDataChannelManager>());
            services.AddSingleton<IOutputDataChannelWriter>(provider => provider.GetRequiredService<OutputDataChannelManager>());

            services.AddHostedService<ProcessWorker>();

            return services;
        }

        private static bool ShouldAutoRegister(Type type)
        {
            if (AutoRegistrationSuffixes.Any(s => type.Name.EndsWith(s, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }
    }
}
