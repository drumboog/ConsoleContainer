using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;

namespace ConsoleContainer.WorkerService.Mappers
{
    public class ProcessMapper : IProcessMapper
    {
        public ProcessInformationDto Map(ProcessInformation pi) =>
            new ProcessInformationDto()
            {
                ProcessLocator = pi.ProcessLocator,
                ProcessId = null,
                ProcessName = pi.ProcessName,
                FilePath = pi.FilePath,
                Arguments = pi.Arguments,
                WorkingDirectory = pi.WorkingDirectory
            };
    }
}
