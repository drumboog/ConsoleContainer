using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;

namespace ConsoleContainer.WorkerService.Mappers
{
    public interface IProcessGroupMapper
    {
        ProcessGroupDto Map(ProcessGroup group);
        ProcessGroupSummaryDto MapSummary(ProcessGroup group);
    }
}
