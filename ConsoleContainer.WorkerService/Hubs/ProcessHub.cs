using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace ConsoleContainer.WorkerService.Hubs
{
    public class ProcessHub(
        IOutputDataChannelReader outputDataChannelReader
    ) : Hub<IProcessHubSubscription>
    {
        public ChannelReader<ProcessOutputDataDto> ProcessOutputDataStream()
        {
            return outputDataChannelReader.GetChannelReader();
        }
    }
}
