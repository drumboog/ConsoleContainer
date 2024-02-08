using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessGroupCreatedEvent
    {
        public ProcessGroupDto ProcessGroup { get; }

        public ProcessGroupCreatedEvent(ProcessGroupDto processGroup)
        {
            ProcessGroup = processGroup;
        }
    }
}
