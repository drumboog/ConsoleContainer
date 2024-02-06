using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService.Client
{
    public interface IProcessHubClient
    {
        IDisposable CreateSubscription(IProcessHubSubscription hub);
    }
}
