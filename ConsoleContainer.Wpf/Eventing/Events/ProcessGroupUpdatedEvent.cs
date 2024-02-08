using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessGroupUpdatedEvent
    {
        public ProcessGroupDto ProcessGroup { get; }

        public ProcessGroupUpdatedEvent(ProcessGroupDto processGroup)
        {
            ProcessGroup = processGroup;
        }
    }
}
