namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessDeletedEvent
    {
        public Guid ProcessGroupId { get; }
        public Guid ProcessLocator { get; }

        public ProcessDeletedEvent(Guid processGroupId, Guid processLocator)
        {
            ProcessGroupId = processGroupId;
            ProcessLocator = processLocator;
        }
    }
}
