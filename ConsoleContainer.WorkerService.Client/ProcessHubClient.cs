using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace ConsoleContainer.WorkerService.Client
{
    public class ProcessHubClient : HubClient, IProcessHubClient
    {
        private bool disposedValue;

        public ProcessHubClient(string hubConnectionUrl, string hubProxyName)
            : base(hubConnectionUrl, hubProxyName)
        {
        }

        public IDisposable CreateSubscription(IProcessHubSubscription hub)
        {
            return HubConnection.Register(hub);
        }

        public IAsyncEnumerable<ProcessOutputDataDto> GetProcessOutputDataStream(CancellationToken cancellationToken = default)
        {
            return HubConnection.StreamAsync<ProcessOutputDataDto>("ProcessOutputDataStream", cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CloseHubAsync().Wait();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
