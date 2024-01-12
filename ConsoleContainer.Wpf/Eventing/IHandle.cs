namespace ConsoleContainer.Wpf.Eventing
{
    public interface IHandle<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
