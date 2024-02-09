using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;

namespace ConsoleContainer.Wpf.ViewModels.Factories
{
    internal class ProcessVmFactory(
        IWorkerServiceClient workerServiceClient,
        IEventAggregator eventAggregator
    ) : IProcessVmFactory
    {
        public ProcessVM Create(Guid processGroupId, Guid processLocator, int? processId, string processName, string filePath, string? arguments, string? workingDirectory, ProcessState state)
        {
            return new ProcessVM(workerServiceClient, eventAggregator, processGroupId, processLocator, processId, processName, filePath, arguments, workingDirectory, state);
        }
    }
}
