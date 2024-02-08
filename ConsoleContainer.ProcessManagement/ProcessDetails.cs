namespace ConsoleContainer.ProcessManagement
{
    public class ProcessDetails
    {
        public Guid ProcessLocator { get; }
        public string FilePath { get; }
        public string? Arguments { get; init; }
        public string? WorkingDirectory { get; init; }

        public ProcessDetails(Guid processLocator, string filePath, string? arguments = null, string? workingDirectory = null)
        {
            ProcessLocator = processLocator;
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }
    }
}
