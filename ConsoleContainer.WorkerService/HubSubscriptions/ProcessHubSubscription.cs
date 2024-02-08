using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ConsoleContainer.WorkerService.HubSubscriptions
{
    public class ProcessHubSubscription(
        IHubContext<ProcessHub, IProcessHubSubscription> processHubContext
    ) : IProcessHubSubscription
    {
        public Task ProcessGroupCreatedAsync(ProcessGroupDto processGroup)
        {
            return processHubContext.Clients.All.ProcessGroupCreatedAsync(processGroup);
        }

        public Task ProcessGroupUpdatedAsync(ProcessGroupDto processGroup)
        {
            return processHubContext.Clients.All.ProcessGroupUpdatedAsync(processGroup);
        }

        public Task ProcessGroupDeletedAsync(Guid processGroupId)
        {
            return processHubContext.Clients.All.ProcessGroupDeletedAsync(processGroupId);
        }

        public Task ProcessCreatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            return processHubContext.Clients.All.ProcessCreatedAsync(processGroupId, process);
        }

        public Task ProcessUpdatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            return processHubContext.Clients.All.ProcessUpdatedAsync(processGroupId, process);
        }

        public Task ProcessStartedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            return processHubContext.Clients.All.ProcessStartedAsync(processGroupId, process);
        }

        public Task ProcessStoppedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            return processHubContext.Clients.All.ProcessStoppedAsync(processGroupId, process);
        }

        public Task ProcessDeletedAsync(Guid processGroupId, Guid processLocator)
        {
            return processHubContext.Clients.All.ProcessDeletedAsync(processGroupId, processLocator);
        }

        public Task ProcessOutputDataReceivedAsync(Guid processLocator, ProcessOutputDataDto data)
        {
            return processHubContext.Clients.All.ProcessOutputDataReceivedAsync(processLocator, data);
        }
    }
}
