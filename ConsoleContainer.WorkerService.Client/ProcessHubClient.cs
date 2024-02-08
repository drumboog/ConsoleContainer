using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace ConsoleContainer.WorkerService.Client
{
    public class ProcessHubClient : HubClient, IProcessHubClient
    {
        public ProcessHubClient(string hubConnectionUrl, string hubProxyName)
            : base(hubConnectionUrl, hubProxyName)
        {
        }

        public IDisposable CreateSubscription(IProcessHubSubscription hub)
        {
            return HubConnection.Register(hub);
        }
    }
}
