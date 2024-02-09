using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;
using ConsoleContainer.ProcessManagement;

namespace ConsoleContainer.WorkerService.Mappers
{
    public class ProcessMapper(
        IProcessManager<ProcessKey> processManager
    ) : IProcessMapper
    {
        public ProcessInformationDto Map(Guid processGroupId, ProcessInformation pi) =>
            new ProcessInformationDto()
            {
                ProcessLocator = pi.ProcessLocator,
                ProcessId = null,
                ProcessName = pi.ProcessName,
                FilePath = pi.FilePath,
                Arguments = pi.Arguments,
                WorkingDirectory = pi.WorkingDirectory,
                State = GetState(processGroupId, pi.ProcessLocator)
            };

        private Contracts.ProcessState GetState(Guid processGroupId, Guid processLocator)
        {
            var process = processManager.GetProcess(new ProcessKey(processGroupId, processLocator));
            if (process is null)
            {
                return Contracts.ProcessState.Idle;
            }
            return Contracts.ProcessState.Idle;
        }
    }
}
