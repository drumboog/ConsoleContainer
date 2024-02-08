using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessStartedEvent
    {
        public Guid ProcessGroupId { get; }
        public ProcessInformationDto ProcessInformation { get; }

        public ProcessStartedEvent(Guid processGroupId, ProcessInformationDto processInformation)
        {
            ProcessGroupId = processGroupId;
            ProcessInformation = processInformation;
        }
    }
}
