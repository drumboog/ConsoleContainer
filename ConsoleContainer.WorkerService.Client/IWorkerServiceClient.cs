using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService.Client
{
    public interface IWorkerServiceClient
    {
        Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken);
    }
}
