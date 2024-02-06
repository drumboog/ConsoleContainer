namespace ConsoleContainer.Contracts
{
    public class ProcessDto
    {
        public string? ProcessLocator { get; set; }
        public int? ProcessId { get; set; }
        public string? FilePath { get; set; }
        public string? Arguments { get; set; }
        public string? WorkingDirectory { get; set; }
    }
}
