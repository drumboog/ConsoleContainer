using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;

namespace ConsoleContainer.WorkerService.Mappers
{
    public interface IProcessMapper
    {
        ProcessInformationDto Map(ProcessInformation pi);
    }
}
