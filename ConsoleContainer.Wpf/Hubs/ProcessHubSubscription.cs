using ConsoleContainer.Contracts;
using ConsoleContainer.Eventing;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;

namespace ConsoleContainer.Wpf.Hubs
{
    internal class ProcessHubSubscription : IProcessHubSubscription
    {
        private readonly IEventAggregator eventAggregator;

        public ProcessHubSubscription(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public Task ProcessAdded(ProcessDto process)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessAddedEvent(process.ProcessLocator ?? string.Empty));
        }

        public Task ProcessStarted(ProcessDto process)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessStartedEvent(process.ProcessLocator ?? string.Empty, process.ProcessId ?? 0));
        }

        public Task ProcessStopped(ProcessDto process)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessStoppedEvent(process.ProcessLocator ?? string.Empty));
        }

        public Task ProcessRemoved(ProcessDto process)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessAddedEvent(process.ProcessLocator ?? string.Empty));
        }
    }
}
