using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace ConsoleContainer.WorkerService.Hubs
{
    public class ProcessHub : Hub<IProcessHubSubscription>
    {
    }
}
