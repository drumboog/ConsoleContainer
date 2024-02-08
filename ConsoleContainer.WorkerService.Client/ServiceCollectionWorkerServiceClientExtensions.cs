using ConsoleContainer.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.WorkerService.Client
{
    public static class ServiceCollectionWorkerServiceClientExtensions
    {
        public static IServiceCollection AddWorkerServiceClient(this IServiceCollection services, Action<IWorkerServiceClientConfiguration> configure)
        {
            var configuration = new WorkerServiceClientConfiguration();

            configure(configuration);

            if (configuration.WorkerServiceUrl is null)
            {
                throw new Exception("WorkerServiceUrl must be configured.");
            }

            services.AddSingleton<IProcessHubClient>(provider =>
            {
                var hubClient = new ProcessHubClient($"{configuration.WorkerServiceUrl}/signalr/Process", "Process");

                foreach (var factory in configuration.ProcessHubSubscriptionFactories)
                {
                    hubClient.CreateSubscription(factory(provider));
                }

                return hubClient;
            });
            services.AddHttpClient<IWorkerServiceClient, WorkerServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.WorkerServiceUrl);
            });

            return services;
        }

        public static void AddProcessHubSubscription<T>(this IWorkerServiceClientConfiguration configuration)
            where T : IProcessHubSubscription
        {
            configuration.AddProcessHubSubscription(provider => provider.GetRequiredService<T>());
        }


        private class WorkerServiceClientConfiguration : IWorkerServiceClientConfiguration
        {
            public string? WorkerServiceUrl { get; private set; }
            public List<Func<IServiceProvider, IProcessHubSubscription>> ProcessHubSubscriptionFactories { get; } = new();

            public void WithWorkerServiceUrl(string workerServiceUrl)
            {
                WorkerServiceUrl = workerServiceUrl;
            }

            public void AddProcessHubSubscription(Func<IServiceProvider, IProcessHubSubscription> factory)
            {
                ProcessHubSubscriptionFactories.Add(factory);
            }
        }
    }
}
