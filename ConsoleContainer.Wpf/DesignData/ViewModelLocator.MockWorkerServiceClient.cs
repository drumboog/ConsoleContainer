using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;

namespace ConsoleContainer.Wpf.DesignData
{
    internal partial class ViewModelLocator
    {
        private class MockWorkerServiceClient : IWorkerServiceClient
        {
            public Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task CreateProcessGroupAsync(ProcessGroupDto processGroup)
            {
                throw new NotImplementedException();
            }

            public Task UpdateProcessGroupAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup)
            {
                throw new NotImplementedException();
            }

            public Task DeleteProcessGroupAsync(Guid processGroupId)
            {
                throw new NotImplementedException();
            }

            public Task CreateProcessAsync(Guid processGroupId, ProcessInformationDto processInformation)
            {
                throw new NotImplementedException();
            }

            public Task UpdateProcessAsync(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto processInformation)
            {
                throw new NotImplementedException();
            }

            public Task StartProcessAsync(Guid processGroupId, Guid processLocator)
            {
                throw new NotImplementedException();
            }

            public Task StartProcessesAsync(Guid processGroupId, IEnumerable<Guid> processLocators)
            {
                throw new NotImplementedException();
            }

            public Task StopProcessAsync(Guid processGroupId, Guid processLocator)
            {
                throw new NotImplementedException();
            }

            public Task StopProcessesAsync(Guid processGroupId, IEnumerable<Guid> processLocators)
            {
                throw new NotImplementedException();
            }

            public Task DeleteProcessAsync(Guid processGroupId, Guid processLocator)
            {
                throw new NotImplementedException();
            }
        }
    }
}
