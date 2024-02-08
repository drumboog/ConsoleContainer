namespace ConsoleContainer.Domain.Contracts
{
    public interface IProcessInformation
    {
        string? ProcessName { get; }
        string? FilePath { get; }
        string? Arguments { get; }
        string? WorkingDirectory { get; }
    }
}
