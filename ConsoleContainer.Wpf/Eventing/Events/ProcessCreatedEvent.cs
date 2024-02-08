using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessCreatedEvent
    {
        public Guid ProcessGroupId { get; }
        public ProcessInformationDto ProcessInformation { get; }

        public ProcessCreatedEvent(Guid processGroupId, ProcessInformationDto processInformation)
        {
            ProcessGroupId = processGroupId;
            ProcessInformation = processInformation;
        }
    }
}
