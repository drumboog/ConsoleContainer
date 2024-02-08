namespace ConsoleContainer.Contracts
{
    public class ProcessGroupSummaryDto : ProcessGroupDto
    {
        public List<ProcessInformationDto> Processes { get; set; } = new List<ProcessInformationDto>();
    }
}
