namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessRemovedEvent
    {
        public string ProcessLocator { get; }

        public ProcessRemovedEvent(string processLocator)
        {
            ProcessLocator = processLocator;
        }
    }
}
