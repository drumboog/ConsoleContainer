using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessUpdatedEvent
    {
        public Guid ProcessGroupId { get; }
        public ProcessInformationDto ProcessInformation { get; }

        public ProcessUpdatedEvent(Guid processGroupId, ProcessInformationDto processInformation)
        {
            ProcessGroupId = processGroupId;
            ProcessInformation = processInformation;
        }
    }
}
