namespace ConsoleContainer.Wpf.Domain.Contracts
{
    public interface IProcessGroup
    {
        string? GroupName { get; }
        IEnumerable<IProcessInformation> Processes { get; }
    }
}
