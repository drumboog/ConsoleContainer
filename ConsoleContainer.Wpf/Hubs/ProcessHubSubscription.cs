﻿using ConsoleContainer.Contracts;
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

        public Task ProcessGroupCreatedAsync(ProcessGroupDto processGroup)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessGroupCreatedEvent(processGroup));
        }

        public Task ProcessGroupUpdatedAsync(ProcessGroupDto processGroup)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessGroupUpdatedEvent(processGroup));
        }

        public Task ProcessGroupDeletedAsync(Guid processGroupId)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessGroupDeletedEvent(processGroupId));
        }

        public Task ProcessCreatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessCreatedEvent(processGroupId, process));
        }

        public Task ProcessUpdatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessUpdatedEvent(processGroupId, process));
        }

        public Task ProcessStateUpdatedAsync(Guid processGroupId, Guid processLocator, ProcessState state, int? processId)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessStateUpdatedEvent(processGroupId, processLocator, state, processId));
        }

        public Task ProcessDeletedAsync(Guid processGroupId, Guid processLocator)
        {
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessDeletedEvent(processGroupId, processLocator));
        }
    }
}
