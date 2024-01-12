namespace ConsoleContainer.Wpf.Domain
{
    public class ProcessInformation
    {
        public string ProcessName { get; }
        public string FileName { get; }
        public string? Arguments { get; init; }
        public string? WorkingDirectory { get; init; }

        public ProcessInformation(string processName, string fileName, string? arguments = null, string? workingDirectory = null)
        {
            ProcessName = processName;
            FileName = fileName;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }
    }
}
