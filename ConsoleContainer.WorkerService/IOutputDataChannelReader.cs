using ConsoleContainer.Contracts;
using System.Threading.Channels;

namespace ConsoleContainer.WorkerService
{
    public interface IOutputDataChannelReader
    {
        ChannelReader<ProcessOutputDataDto> GetChannelReader();
    }
}
