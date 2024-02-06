namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ProcessAddedEvent
    {
        public string ProcessLocator { get; }

        public ProcessAddedEvent(string processLocator)
        {
            ProcessLocator = processLocator;
        }
    }
}
