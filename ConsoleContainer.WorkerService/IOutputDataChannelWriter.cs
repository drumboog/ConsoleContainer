using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService
{
    public interface IOutputDataChannelWriter
    {
        Task WriteOutputDataAsync(ProcessOutputDataDto outputData);
    }
}
