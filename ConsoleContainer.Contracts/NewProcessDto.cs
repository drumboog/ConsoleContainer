namespace ConsoleContainer.Contracts
{
    public class NewProcessDto
    {
        public string? ProcessLocator { get; set; }
        public string? FilePath { get; set; }
        public string? Arguments { get; set; }
        public string? WorkingDirectory { get; set; }
    }
}
