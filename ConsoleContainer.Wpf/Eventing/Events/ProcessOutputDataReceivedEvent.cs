using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessOutputDataReceivedEvent
    {
        public Guid ProcessGroupId { get; }
        public ProcessOutputDataDto Data { get; }

        public ProcessOutputDataReceivedEvent(Guid processGroupId, ProcessOutputDataDto data)
        {
            ProcessGroupId = processGroupId;
            Data = data;
        }
    }
}
