using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleContainer.WorkerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessGroupController(
        ILogger<ProcessGroupController> logger,
        IProcessGroupService processGroupService
    ) : ControllerBase
    {
        [HttpGet(Name = "GetProcessGroups")]
        public async Task<IEnumerable<ProcessGroupSummaryDto>> GetAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving Process Groups");
            return await processGroupService.GetProcessGroupsAsync(cancellationToken);
        }

        [HttpPost(Name = "CreateProcessGroup")]
        public async Task CreateAsync(ProcessGroupDto processGroup)
        {
            logger.LogInformation("Creating Process Group");
            await processGroupService.CreateProcessGroupAsync(processGroup);
        }

        [HttpPut("{processGroupId}", Name = "UpdateProcessGroup")]
        public async Task UpdateAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup)
        {
            logger.LogInformation("Creating Process Group");
            await processGroupService.UpdateProcessGroupAsync(processGroupId, processGroup);
        }

        [HttpDelete("{processGroupId}", Name = "DeleteProcessGroup")]
        public async Task DeleteAsync(Guid processGroupId)
        {
            logger.LogInformation("Deleting Process Group");
            await processGroupService.DeleteProcessGroupAsync(processGroupId);
        }
    }
}
