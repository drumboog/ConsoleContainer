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
            logger.LogInformation($"ProcessGroupId: {processGroupId}, ProcessLocator: {process.ProcessLocator} - Creating Process");
            await processGroupService.CreateProcessAsync(processGroupId, process);
        }

        [HttpPut("{processLocator}", Name = "UpdateProcess")]
        public async Task Update(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto process)
        {
            logger.LogInformation($"ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator} - Updating Process");
            await processGroupService.UpdateProcessAsync(processGroupId, processLocator, process);
        }

        [HttpPut("{processLocator}/start", Name = "StartProcess")]
        public async Task Start(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation($"ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator} - Starting Process");
            await processGroupService.StartProcessAsync(processGroupId, processLocator);
        }

        [HttpPut("start", Name = "StartProcesses")]
        public async Task StartMultiple(Guid processGroupId, ProcessStartStopCollectionDto processCollection)
        {
            var processLocators = processCollection.Processes.Select(x => x.ProcessLocator);
            logger.LogInformation($"ProcessGroupId: {processGroupId} - Starting Processes: {string.Join(", ", processLocators)}");
            await processGroupService.StartProcessesAsync(processGroupId, processLocators);
        }

        [HttpPut("{processLocator}/stop", Name = "StopProcess")]
        public async Task Stop(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation($"ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator} - Stopping Process");
            await processGroupService.StopProcessAsync(processGroupId, processLocator);
        }

        [HttpPut("stop", Name = "StopProcesses")]
        public async Task StopMultiple(Guid processGroupId, ProcessStartStopCollectionDto processCollection)
        {
            var processLocators = processCollection.Processes.Select(x => x.ProcessLocator);
            logger.LogInformation($"ProcessGroupId: {processGroupId} - Stopping Processes: {string.Join(", ", processLocators)}");
            await processGroupService.StopProcessesAsync(processGroupId, processLocators);
        }

        [HttpDelete("{processLocator}", Name = "DeleteProcess")]
        public async Task Delete(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation($"ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator} - Deleting Process");
            await processGroupService.DeleteProcessAsync(processGroupId, processLocator);
        }
    }
}
