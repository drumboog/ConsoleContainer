namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessGroupDeletedEvent
    {
        public Guid ProcessGroupId { get; }

        public ProcessGroupDeletedEvent(Guid processGroupId)
        {
            ProcessGroupId = processGroupId;
        }
    }
}
