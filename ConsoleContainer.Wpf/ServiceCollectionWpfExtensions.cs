using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Hubs;
using ConsoleContainer.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.Wpf
{
    public static class ServiceCollectionWpfExtensions
    {
        public static IServiceCollection AddWpf(this IServiceCollection services)
        {
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddSingleton<ProcessHubSubscription>();
            services.AddSingleton<IDialogService, DialogService>();

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(filter => filter.AssignableTo<ViewModel>())
                    .AsSelf()
                    .WithTransientLifetime();
            });

            services.AddWorkerServiceClient(config =>
            {
                config.WithWorkerServiceUrl("https://localhost:7276");
                config.AddProcessHubSubscription<ProcessHubSubscription>();
            });

            return services;
        }
    }
}
