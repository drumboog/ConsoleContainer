namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessStartedEvent
    {
        public string ProcessLocator { get; }
        public int ProcessId { get; }

        public ProcessStartedEvent(string processLocator, int processId)
        {
            ProcessLocator = processLocator;
            ProcessId = processId;
        }
    }
}
