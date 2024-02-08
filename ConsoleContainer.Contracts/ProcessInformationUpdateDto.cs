using ConsoleContainer.Domain.Contracts;

namespace ConsoleContainer.Contracts
{
    public class ProcessInformationUpdateDto : IProcessInformation
    {
        public int? ProcessId { get; set; }
        public string? ProcessName { get; set; }
        public string? FilePath { get; set; }
        public string? Arguments { get; set; }
        public string? WorkingDirectory { get; set; }
    }
}
