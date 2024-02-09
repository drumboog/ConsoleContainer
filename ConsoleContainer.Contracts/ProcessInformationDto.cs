namespace ConsoleContainer.Contracts
{
    public class ProcessInformationDto : ProcessInformationUpdateDto
    {
        public Guid ProcessLocator { get; set; }
        public int? ProcessId { get; set; }
        public ProcessState State { get; set; }
    }
}
