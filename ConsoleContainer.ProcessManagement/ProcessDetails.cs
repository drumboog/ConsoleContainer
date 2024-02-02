namespace ConsoleContainer.ProcessManagement
{
    public class ProcessDetails
    {
        public string ProcessLocator { get; }
        public string FileName { get; }
        public string? Arguments { get; init; }
        public string? WorkingDirectory { get; init; }

        public ProcessDetails(string processLocator, string fileName, string? arguments = null, string? workingDirectory = null)
        {
            ProcessLocator = processLocator;
            FileName = fileName;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }
    }
}
