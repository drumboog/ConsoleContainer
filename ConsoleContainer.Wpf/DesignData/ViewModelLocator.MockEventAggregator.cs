using ConsoleContainer.Wpf.Eventing;

namespace ConsoleContainer.Wpf.DesignData
{
    internal partial class ViewModelLocator
    {
        private class MockEventAggregator : IEventAggregator
        {
            public bool HandlerExistsFor(Type messageType)
            {
                return true;
            }

            public Task PublishAsync(object message, Func<Func<Task>, Task> marshal, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public void Subscribe(object subscriber, Func<Func<Task>, Task> marshal)
            {
            }

            public void Unsubscribe(object subscriber)
            {
            }
        }
    }
}
