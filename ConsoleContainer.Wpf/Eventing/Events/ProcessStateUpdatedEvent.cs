using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessStateUpdatedEvent
    {
        public Guid ProcessGroupId { get; }
        public Guid ProcessLocator { get; }
        public ProcessState State { get; }
        public int? ProcessId { get; }

        public ProcessStateUpdatedEvent(
            Guid processGroupId,
            Guid processLocator,
            ProcessState state,
            int? processId)
        {
            ProcessGroupId = processGroupId;
            ProcessLocator = processLocator;
            State = state;
            ProcessId = processId;
        }
    }
}
