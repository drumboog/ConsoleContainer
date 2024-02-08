using ConsoleContainer.ProcessManagement.Events;

namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessWrapper
    {
        event EventHandler<ProcessOutputDataEventArgs>? OutputDataReceived;
        event EventHandler<ProcessStateChangedEventArgs>? StateChanged;

        int? ProcessId { get; }

        Guid ProcessLocator { get; }
        ProcessDetails ProcessDetails { get; }

        ProcessState State { get; }

        IReadOnlyCollection<ProcessOutputData> OutputData { get; }

        Task StartProcessAsync();
        Task StopProcessAsync();
    }
}
