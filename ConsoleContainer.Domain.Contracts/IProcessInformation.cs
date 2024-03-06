namespace ConsoleContainer.Domain.Contracts
{
    public interface IProcessInformation
    {
        string ProcessName { get; }
        string FilePath { get; }
        string? Arguments { get; }
        string? WorkingDirectory { get; }
        bool AutoStart { get; }
        bool RestartOnError { get; }
        bool RestartOnExit { get; }
    }
}
