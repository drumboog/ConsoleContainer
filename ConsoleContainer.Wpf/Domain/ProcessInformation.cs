namespace ConsoleContainer.Wpf.Domain
{
    public class ProcessInformation
    {
        public string ProcessName { get; }
        public string FilePath { get; }
        public string? Arguments { get; init; }
        public string? WorkingDirectory { get; init; }

        public ProcessInformation(string processName, string filePath, string? arguments = null, string? workingDirectory = null)
        {
            ProcessName = processName;
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }
    }
}
