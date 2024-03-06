using ConsoleContainer.Domain.Contracts;
using ConsoleContainer.Kernel.Validation;
using Newtonsoft.Json;

namespace ConsoleContainer.Domain
{
    public class ProcessInformation
    {
        public Guid ProcessLocator { get; }
        public string ProcessName { get; private set; }
        public string FilePath { get; private set; }
        public string? Arguments { get; private set; }
        public string? WorkingDirectory { get; private set; }
        public bool AutoStart { get; private set; }
        public bool RestartOnError { get; private set; }
        public bool RestartOnExit { get; private set; }

        [JsonConstructor]
        public ProcessInformation(
            Guid processLocator,
            string processName,
            string filePath,
            string? arguments = null,
            string? workingDirectory = null,
            bool autoStart = false,
            bool restartOnError = false,
            bool restartOnExit = false
        )
        {
            ProcessLocator = processLocator;
            ProcessName = processName;
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
            AutoStart = autoStart;
            RestartOnError = restartOnError;
            RestartOnExit = restartOnExit;
        }

        public ProcessInformation(Guid processLocator, IProcessInformation processInformation)
            : this(
                  processLocator,
                  processInformation.ProcessName.Required(),
                  processInformation.FilePath.Required(),
                  processInformation.Arguments,
                  processInformation.WorkingDirectory,
                  processInformation.AutoStart,
                  processInformation.RestartOnError,
                  processInformation.RestartOnExit
            )
        {
        }
    }
}
