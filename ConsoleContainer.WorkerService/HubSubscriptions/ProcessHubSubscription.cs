using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace ConsoleContainer.WorkerService.HubSubscriptions
{
    public class ProcessHubSubscription(
        ILogger<ProcessHubSubscription> logger,
        IHubContext<ProcessHub, IProcessHubSubscription> processHubContext
    ) : IProcessHubSubscription
    {
        public Task ProcessGroupCreatedAsync(ProcessGroupDto processGroup)
        {
            logger.LogInformation($"ProcessGroupCreatedAsync - ProcessGroupId: {processGroup.ProcessGroupId}");
            return processHubContext.Clients.All.ProcessGroupCreatedAsync(processGroup);
        }

        public Task ProcessGroupUpdatedAsync(ProcessGroupDto processGroup)
        {
            logger.LogInformation($"ProcessGroupUpdatedAsync - ProcessGroupId: {processGroup.ProcessGroupId}");
            return processHubContext.Clients.All.ProcessGroupUpdatedAsync(processGroup);
        }

        public Task ProcessGroupDeletedAsync(Guid processGroupId)
        {
            logger.LogInformation($"ProcessGroupDeletedAsync - ProcessGroupId: {processGroupId}");
            return processHubContext.Clients.All.ProcessGroupDeletedAsync(processGroupId);
        }

        public Task ProcessCreatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            logger.LogInformation($"ProcessCreatedAsync - ProcessGroupId: {processGroupId}, ProcessLocator: {process.ProcessLocator}");
            return processHubContext.Clients.All.ProcessCreatedAsync(processGroupId, process);
        }

        public Task ProcessUpdatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            logger.LogInformation($"ProcessUpdatedAsync - ProcessGroupId: {processGroupId}, ProcessLocator: {process.ProcessLocator}");
            return processHubContext.Clients.All.ProcessUpdatedAsync(processGroupId, process);
        }

        public Task ProcessStateUpdatedAsync(Guid processGroupId, Guid processLocator, ProcessState state, int? processId)
        {
            logger.LogInformation($"ProcessStateUpdatedAsync - ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator}");
            return processHubContext.Clients.All.ProcessStateUpdatedAsync(processGroupId, processLocator, state, processId);
        }

        public Task ProcessDeletedAsync(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation($"ProcessDeletedAsync - ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator}");
            return processHubContext.Clients.All.ProcessDeletedAsync(processGroupId, processLocator);
        }
    }
}
