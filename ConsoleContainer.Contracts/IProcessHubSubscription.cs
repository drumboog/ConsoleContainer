namespace ConsoleContainer.Contracts
{
    public interface IProcessHubSubscription
    {
        Task ProcessGroupCreatedAsync(ProcessGroupDto processGroup);
        Task ProcessGroupUpdatedAsync(ProcessGroupDto processGroup);
        Task ProcessGroupDeletedAsync(Guid processGroupId);
        Task ProcessCreatedAsync(Guid processGroupId, ProcessInformationDto process);
        Task ProcessUpdatedAsync(Guid processGroupId, ProcessInformationDto process);
        Task ProcessStartedAsync(Guid processGroupId, ProcessInformationDto process);
        Task ProcessStoppedAsync(Guid processGroupId, ProcessInformationDto process);
        Task ProcessDeletedAsync(Guid processGroupId, Guid processLocator);
        Task ProcessOutputDataReceivedAsync(Guid processLocator, ProcessOutputDataDto data);
    }
}
