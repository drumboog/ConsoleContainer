namespace ConsoleContainer.Contracts
{
    public interface IProcessHubSubscription
    {
        Task ProcessAdded(ProcessDto process);
        Task ProcessStarted(ProcessDto process);
        Task ProcessStopped(ProcessDto process);
        Task ProcessRemoved(ProcessDto process);
    }
}
