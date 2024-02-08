using ConsoleContainer.Domain.Contracts;
using ConsoleContainer.Kernel.Validation;
using System.Text.Json.Serialization;

namespace ConsoleContainer.Domain
{
    public class ProcessInformation
    {
        public Guid ProcessLocator { get; }
        public string ProcessName { get; private set; }
        public string FilePath { get; private set; }
        public string? Arguments { get; private set; }
        public string? WorkingDirectory { get; private set; }

        [JsonConstructor]
        public ProcessInformation(Guid processLocator, string processName, string filePath, string? arguments = null, string? workingDirectory = null)
        {
            ProcessLocator = processLocator;
            ProcessName = processName;
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }

        public ProcessInformation(Guid processLocator, IProcessInformation processInformation)
            : this(
                  processLocator,
                  processInformation.ProcessName.Required(),
                  processInformation.FilePath.Required(),
                  processInformation.Arguments,
                  processInformation.WorkingDirectory
            )
        {
        }
    }
}
