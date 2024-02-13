using ConsoleContainer.Contracts;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessOutputDataReceivedEvent
    {
        public ProcessOutputDataDto Data { get; }

        public ProcessOutputDataReceivedEvent(ProcessOutputDataDto data)
        {
            Data = data;
        }
    }
}
