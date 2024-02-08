// Source based on https://github.com/Caliburn-Micro/Caliburn.Micro

namespace ConsoleContainer.Wpf.Eventing
{
    public interface IHandle<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
