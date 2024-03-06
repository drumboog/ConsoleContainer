namespace ConsoleContainer.ProcessManagement
{
    public class ProcessDetails
    {
        public string FilePath { get; }
        public string? Arguments { get; }
        public string? WorkingDirectory { get; }
        public bool RestartOnError { get; }
        public bool RestartOnExit { get; }

        public ProcessDetails(string filePath, string? arguments = null, string? workingDirectory = null, bool restartOnError = false, bool restartOnExit = false)
        {
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
            RestartOnError = restartOnError;
            RestartOnExit = restartOnExit;
        }
    }
}
