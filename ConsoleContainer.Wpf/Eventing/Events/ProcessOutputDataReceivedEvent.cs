using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessOutputDataReceivedEvent
    {
        public Guid ProcessGroupId { get; }
        public Guid ProcessLocator { get; }
        public ProcessOutputDataDto Data { get; }

        public ProcessOutputDataReceivedEvent(Guid processGroupId, Guid processLocator, ProcessOutputDataDto data)
        {
            ProcessGroupId = processGroupId;
            ProcessLocator = processLocator;
            Data = data;
        }
    }
}
