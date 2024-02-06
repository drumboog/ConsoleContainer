using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace ConsoleContainer.WorkerService.Hubs
{
    public class ProcessHub : Hub<IProcessHubSubscription>
    {
        public async Task AddProcess(NewProcessDto process)
        {
            await Clients.All.ProcessAdded(
                new ProcessDto()
                {
                    ProcessLocator = process.ProcessLocator,
                    ProcessId = 123456,
                    FilePath = process.FilePath,
                    Arguments = process.Arguments,
                    WorkingDirectory = process.WorkingDirectory
                }
            );
        }
    }
}
