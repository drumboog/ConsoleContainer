using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService.Client
{
    public interface IWorkerServiceClient
    {
        Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken);
        Task CreateProcessGroupAsync(ProcessGroupDto processGroup);
        Task UpdateProcessGroupAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup);
        Task DeleteProcessGroupAsync(Guid processGroupId);

        Task CreateProcessAsync(Guid processGroupId, ProcessInformationDto processInformation);
        Task UpdateProcessAsync(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto processInformation);
        Task StartProcessAsync(Guid processGroupId, Guid processLocator);
        Task StartProcessesAsync(Guid processGroupId, IEnumerable<Guid> processLocators);
        Task StopProcessAsync(Guid processGroupId, Guid processLocator);
        Task StopProcessesAsync(Guid processGroupId, IEnumerable<Guid> processLocators);
        Task DeleteProcessAsync(Guid processGroupId, Guid processLocator);
    }
}
