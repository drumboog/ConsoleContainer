using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessStoppedEvent
    {
        public Guid ProcessGroupId { get; }
        public ProcessInformationDto ProcessInformation { get; }

        public ProcessStoppedEvent(Guid processGroupId, ProcessInformationDto processInformation)
        {
            ProcessGroupId = processGroupId;
            ProcessInformation = processInformation;
        }
    }
}
