using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Configuration;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Hubs;
using ConsoleContainer.Wpf.Services;
using ConsoleContainer.Wpf.ViewModels;
using ConsoleContainer.Wpf.ViewModels.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.Wpf
{
    public static class ServiceCollectionWpfExtensions
    {
        public static IServiceCollection AddWpf(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            services.AddSingleton<MainWindow>();

            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddSingleton<ProcessHubSubscription>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ProcessContainerVM>();
            services.AddTransient<IProcessGroupVmFactory, ProcessGroupVmFactory>();
            services.AddTransient<IProcessVmFactory, ProcessVmFactory>();

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(filter => filter.AssignableTo<ViewModel>())
                    .AsSelf()
                    .WithTransientLifetime();
            });

            services.AddWorkerServiceClient(config =>
            {
                config.WithWorkerServiceUrl(applicationSettings.WorkerServiceUrl);
                config.AddProcessHubSubscription<ProcessHubSubscription>();
            });

            return services;
        }
    }
}
