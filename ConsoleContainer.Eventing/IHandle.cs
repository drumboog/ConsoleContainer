// Source obtained from https://github.com/Caliburn-Micro/Caliburn.Micro

namespace ConsoleContainer.Eventing
{
    public interface IHandle<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
