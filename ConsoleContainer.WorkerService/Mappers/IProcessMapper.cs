using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;

namespace ConsoleContainer.WorkerService.Mappers
{
    public interface IProcessMapper
    {
        ProcessInformationDto Map(Guid processGroupId, ProcessInformation pi);
    }
}
