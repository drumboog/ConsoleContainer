using ConsoleContainer.Domain.Contracts;

namespace ConsoleContainer.Contracts
{
    public class ProcessInformationDto : ProcessInformationUpdateDto
    {
        public Guid? ProcessLocator { get; set; }
    }
}
