using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace ConsoleContainer.WorkerService.Hubs
{
    public class ProcessHub(
        ILogger<ProcessHub> logger,
        IOutputDataChannelReader outputDataChannelReader
    ) : Hub<IProcessHubSubscription>
    {
        public ChannelReader<ProcessOutputDataDto> ProcessOutputDataStream()
        {
            return outputDataChannelReader.GetChannelReader();
        }

        public override Task OnConnectedAsync()
        {
            logger.LogInformation("Connection initiated");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (exception is null)
            {
                logger.LogInformation("Connection disconnected");
            }
            else
            {
                logger.LogError(exception, "Connection disconnected");
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
