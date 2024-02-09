using ConsoleContainer.ProcessManagement.Events;

namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessWrapper<TKey>
    {
        event EventHandler<ProcessOutputDataEventArgs<TKey>>? OutputDataReceived;
        event EventHandler<ProcessStateChangedEventArgs<TKey>>? StateChanged;

        TKey Key { get; }

        int? ProcessId { get; }

        ProcessDetails ProcessDetails { get; }

        ProcessState State { get; }

        IReadOnlyCollection<ProcessOutputData> OutputData { get; }

        Task<bool> StartProcessAsync();
        Task<bool> StopProcessAsync();

        Task UpdateProcessDetails(ProcessDetails processDetails);
    }
}
