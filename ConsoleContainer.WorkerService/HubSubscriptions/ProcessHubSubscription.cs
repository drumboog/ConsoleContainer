using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ConsoleContainer.WorkerService.HubSubscriptions
{
    public class ProcessHubSubscription(
        IHubContext<ProcessHub, IProcessHubSubscription> processHubContext
    ) : IProcessHubSubscription
    {
        public Task ProcessAdded(ProcessDto process)
        {
            return processHubContext.Clients.All.ProcessAdded(process);
        }

        public Task ProcessStarted(ProcessDto process)
        {
            return processHubContext.Clients.All.ProcessStarted(process);
        }

        public Task ProcessStopped(ProcessDto process)
        {
            return processHubContext.Clients.All.ProcessStopped(process);
        }

        public Task ProcessRemoved(ProcessDto process)
        {
            return processHubContext.Clients.All.ProcessRemoved(process);
        }
    }
}
