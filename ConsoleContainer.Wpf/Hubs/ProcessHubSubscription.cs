using ConsoleContainer.Contracts;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using Microsoft.Extensions.Logging;

namespace ConsoleContainer.Wpf.Hubs
{
    internal class ProcessHubSubscription(
        ILogger<ProcessHubSubscription> logger,
        IEventAggregator eventAggregator
    ) : IProcessHubSubscription
    {
        public Task ProcessGroupCreatedAsync(ProcessGroupDto processGroup)
        {
            logger.LogInformation($"Process Group Created - ProcessGroupId: {processGroup.ProcessGroupId}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessGroupCreatedEvent(processGroup));
        }

        public Task ProcessGroupUpdatedAsync(ProcessGroupDto processGroup)
        {
            logger.LogInformation($"Process Group Updated - ProcessGroupId: {processGroup.ProcessGroupId}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessGroupUpdatedEvent(processGroup));
        }

        public Task ProcessGroupDeletedAsync(Guid processGroupId)
        {
            logger.LogInformation($"Process Group Deleted - ProcessGroupId: {processGroupId}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessGroupDeletedEvent(processGroupId));
        }

        public Task ProcessCreatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            logger.LogInformation($"Process Created - ProcessGroupId: {processGroupId}, ProcessLocator: {process.ProcessLocator}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessCreatedEvent(processGroupId, process));
        }

        public Task ProcessUpdatedAsync(Guid processGroupId, ProcessInformationDto process)
        {
            logger.LogInformation($"Process Updated - ProcessGroupId: {processGroupId}, ProcessLocator: {process.ProcessLocator}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessUpdatedEvent(processGroupId, process));
        }

        public Task ProcessStateUpdatedAsync(Guid processGroupId, Guid processLocator, ProcessState state, int? processId)
        {
            logger.LogInformation($"Process State Updated - ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator}, State: {state}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessStateUpdatedEvent(processGroupId, processLocator, state, processId));
        }

        public Task ProcessDeletedAsync(Guid processGroupId, Guid processLocator)
        {
            logger.LogInformation($"Process Deleted - ProcessGroupId: {processGroupId}, ProcessLocator: {processLocator}");
            return eventAggregator.PublishOnCurrentThreadAsync(new ProcessDeletedEvent(processGroupId, processLocator));
        }
    }
}
