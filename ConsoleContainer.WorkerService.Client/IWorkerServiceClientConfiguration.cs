using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService.Client
{
    public interface IWorkerServiceClientConfiguration
    {
        void WithWorkerServiceUrl(string workerServiceUrl);
        void AddProcessHubSubscription(Func<IServiceProvider, IProcessHubSubscription> factory);
    }
}
