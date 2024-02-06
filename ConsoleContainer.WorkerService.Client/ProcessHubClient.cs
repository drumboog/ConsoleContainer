using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace ConsoleContainer.WorkerService.Client
{
    public class ProcessHubClient : HubClient<IProcessHub>, IProcessHub, IProcessHubClient
    {
        public ProcessHubClient(string hubConnectionUrl, string hubProxyName)
            : base(hubConnectionUrl, hubProxyName)
        {
        }

        public Task AddProcess(NewProcessDto process)
        {
            return Hub.AddProcess(process);
        }

        public IDisposable CreateSubscription(IProcessHubSubscription hub)
        {
            return HubConnection.Register(hub);
        }

        protected override IProcessHub CreateHub(HubConnection connection)
        {
            return connection.CreateHubProxy<IProcessHub>();
        }
    }
}
