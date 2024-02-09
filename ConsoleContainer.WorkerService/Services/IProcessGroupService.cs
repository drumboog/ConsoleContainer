﻿using ConsoleContainer.Contracts;

namespace ConsoleContainer.WorkerService.Services
{
    public interface IProcessGroupService
    {
        Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken);
        Task CreateProcessGroupAsync(ProcessGroupDto processGroup);
        Task<bool> UpdateProcessGroupAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup);
        Task<bool> DeleteProcessGroupAsync(Guid processGroupId);
        Task<bool> CreateProcessAsync(Guid processGroupId, ProcessInformationDto processInformation);
        Task<bool> UpdateProcessAsync(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto processInformation);
        Task<bool> DeleteProcessAsync(Guid processGroupId, Guid processLocator);
    }
}