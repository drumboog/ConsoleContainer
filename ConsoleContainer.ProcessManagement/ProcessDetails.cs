namespace ConsoleContainer.ProcessManagement
{
    public class ProcessDetails
    {
        public string FilePath { get; }
        public string? Arguments { get; init; }
        public string? WorkingDirectory { get; init; }

        public ProcessDetails(string filePath, string? arguments = null, string? workingDirectory = null)
        {
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }
    }
}
