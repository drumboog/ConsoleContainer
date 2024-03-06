using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;
using ConsoleContainer.ProcessManagement;

namespace ConsoleContainer.WorkerService.Mappers
{
    public class ProcessMapper(
        IProcessManager<ProcessKey> processManager
    ) : IProcessMapper
    {
        public ProcessInformationDto Map(Guid processGroupId, ProcessInformation pi)
        {
            var process = processManager.GetProcess(new ProcessKey(processGroupId, pi.ProcessLocator));
            return new ProcessInformationDto()
            {
                ProcessLocator = pi.ProcessLocator,
                ProcessId = process?.ProcessId,
                ProcessName = pi.ProcessName,
                FilePath = pi.FilePath,
                Arguments = pi.Arguments,
                WorkingDirectory = pi.WorkingDirectory,
                AutoStart = pi.AutoStart,
                RestartOnError = pi.RestartOnError,
                RestartOnExit = pi.RestartOnExit,
                State = (Contracts.ProcessState)(process?.State ?? ProcessManagement.ProcessState.Idle)
            };
        }
    }
}
