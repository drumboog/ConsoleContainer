namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessStoppedEvent
    {
        public string ProcessLocator { get; }

        public ProcessStoppedEvent(string processLocator)
        {
            ProcessLocator = processLocator;
        }
    }
}
