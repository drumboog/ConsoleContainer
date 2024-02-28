using ConsoleContainer.WorkerService.Client;

namespace ConsoleContainer.Wpf.ViewModels.Factories
{
    internal class ProcessGroupVmFactory(
        IWorkerServiceClient workerServiceClient
    ) : IProcessGroupVmFactory
    {
        public ProcessGroupVM Create(Guid processGroupId, string groupName)
        {
            return new ProcessGroupVM(processGroupId, groupName, workerServiceClient);
        }
    }
}
