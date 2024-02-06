using ConsoleContainer.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleContainer.WorkerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessController : ControllerBase
    {
        private readonly ILogger<ProcessController> _logger;

        public ProcessController(ILogger<ProcessController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProcesses")]
        public IEnumerable<ProcessDto> Get()
        {
            var rand = new Random();
            return Enumerable.Range(1, 5).Select(index => {
                var id = rand.Next(10000, 100000);
                return new ProcessDto
                {
                    ProcessLocator = $"Process{id}",
                    ProcessId = id,
                    FilePath = $@"C:\Processes\{id}\{id}.exe",
                    Arguments = $"Argument {id}",
                    WorkingDirectory = $@"C:\Processes\{id}"
                };
            })
            .ToArray();
        }
    }
}
