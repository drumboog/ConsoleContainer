using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService.Client
{
    public interface IProcessHubClient : IDisposable
    {
        IDisposable CreateSubscription(IProcessHubSubscription hub);
        Task StartAsync();
    }
}
