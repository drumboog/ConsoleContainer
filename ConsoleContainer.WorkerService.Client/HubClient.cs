using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleContainer.WorkerService.Client
{
    public abstract class HubClient
    {
        private readonly ILogger<HubClient> logger;

        protected HubConnection HubConnection { get; }

        public string HubConnectionUrl { get; }
        public string HubProxyName { get; }

        public HubConnectionState State
        {
            get { return HubConnection.State; }
        }

        protected HubClient(ILogger<HubClient> logger, string hubConnectionUrl, string hubProxyName)
        {
            this.logger = logger;
            HubConnectionUrl = hubConnectionUrl;
            HubProxyName = hubProxyName;

            HubConnection = CreateConnection();
        }

        private HubConnection CreateConnection()
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(HubConnectionUrl)
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();

            hubConnection.Reconnected += HubConnection_Reconnected;
            hubConnection.Reconnecting += HubConnection_Reconnecting;
            hubConnection.Closed += HubConnection_Closed;

            return hubConnection;
        }

        public async Task CloseHubAsync()
        {
            await HubConnection.StopAsync().ConfigureAwait(false);
            await HubConnection.DisposeAsync().ConfigureAwait(false);
        }

        public Task StartAsync()
        {
            return HubConnection.StartAsync();
        }

        protected async Task StartHubInternalAsync()
        {
            try
            {
                await HubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not start hub.");
            }
        }

        private Task HubConnection_Closed(Exception? arg)
        {
            return Task.CompletedTask;
        }

        private Task HubConnection_Reconnecting(Exception? ex)
        {
            //HubClientEvents.Log.ClientEvents("_hubConnection_Reconnecting New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
            return Task.CompletedTask;
        }

        private Task HubConnection_Reconnected(string? connectionId)
        {
            //HubClientEvents.Log.ClientEvents("_hubConnection_Reconnected New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
            return Task.CompletedTask;
        }
    }

    public abstract class HubClient<T> : HubClient
    {
        protected T Hub { get; }

        protected HubClient(ILogger<HubClient> logger, string hubConnectionUrl, string hubProxyName)
            : base(logger, hubConnectionUrl, hubProxyName)
        {
            Hub = CreateHub(HubConnection);
        }

        protected abstract T CreateHub(HubConnection connection);
    }
}
