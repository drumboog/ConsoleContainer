using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleContainer.WorkerService.Controllers
{
    [ApiController]
    [Route("ProcessGroup/{processGroupId}/[controller]")]
    public class ProcessController(
        ILogger<ProcessController> logger,
        IProcessGroupService processGroupService
    ) : ControllerBase
    {
        [HttpPost(Name = "CreateProcess")]
        public async Task Create(Guid processGroupId, ProcessInformationDto process)
        {
            logger.LogInformation("Creating Process");
            await processGroupService.CreateProcessAsync(processGroupId, process);
        }

        [HttpPut("{processLocator}", Name = "UpdateProcess")]
        public async Task Update(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto process)
        {
            logger.LogInformation("Updating Process");
            await processGroupService.UpdateProcessAsync(processGroupId, processLocator, process);
        }

        [HttpPut("{processLocator}/start", Name = "StartProcess")]
        public async Task Start(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation("Starting Process");
            await processGroupService.StartProcessAsync(processGroupId, processLocator);
        }

        [HttpPut("{processLocator}/stop", Name = "StopProcess")]
        public async Task Stop(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation("Stopping Process");
            await processGroupService.StopProcessAsync(processGroupId, processLocator);
        }

        [HttpDelete("{processLocator}", Name = "DeleteProcess")]
        public async Task Delete(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation("Deleting Process");
            await processGroupService.DeleteProcessAsync(processGroupId, processLocator);
        }
    }
}
