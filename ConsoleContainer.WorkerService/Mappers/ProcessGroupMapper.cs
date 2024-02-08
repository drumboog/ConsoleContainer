using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;

namespace ConsoleContainer.WorkerService.Mappers
{
    public class ProcessGroupMapper(
        IProcessMapper processMapper
    ) : IProcessGroupMapper
    {
        public ProcessGroupDto Map(ProcessGroup group) =>
            new ProcessGroupDto()
            {
                ProcessGroupId = group.ProcessGroupId,
                GroupName = group.GroupName
            };

        public ProcessGroupSummaryDto MapSummary(ProcessGroup group) =>
            new ProcessGroupSummaryDto()
            {
                ProcessGroupId = group.ProcessGroupId,
                GroupName = group.GroupName,
                Processes = group.Processes.Select(p => processMapper.Map(p)).ToList()
            };
    }
}
