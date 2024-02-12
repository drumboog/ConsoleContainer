namespace ConsoleContainer.Contracts
{
    public interface IProcessHubSubscription
    {
        Task ProcessGroupCreatedAsync(ProcessGroupDto processGroup);
        Task ProcessGroupUpdatedAsync(ProcessGroupDto processGroup);
        Task ProcessGroupDeletedAsync(Guid processGroupId);
        Task ProcessCreatedAsync(Guid processGroupId, ProcessInformationDto process);
        Task ProcessUpdatedAsync(Guid processGroupId, ProcessInformationDto process);
        Task ProcessStateUpdatedAsync(Guid processGroupId, Guid processLocator, ProcessState state, int? processId);
        Task ProcessDeletedAsync(Guid processGroupId, Guid processLocator);
        Task ProcessOutputDataReceivedAsync(Guid processGroupId, Guid processLocator, ProcessOutputDataDto data);
    }
}
