using ConsoleContainer.Contracts;
using System.Threading.Channels;

namespace ConsoleContainer.WorkerService
{
    public sealed class OutputDataChannelManager : IOutputDataChannelReader, IOutputDataChannelWriter
    {
        private readonly Channel<ProcessOutputDataDto> channel = Channel.CreateBounded<ProcessOutputDataDto>(500);

        public ChannelReader<ProcessOutputDataDto> GetChannelReader() => channel.Reader;

        public async Task WriteOutputDataAsync(ProcessOutputDataDto outputData)
        {
            await channel.Writer.WriteAsync(outputData);
        }
    }
}
