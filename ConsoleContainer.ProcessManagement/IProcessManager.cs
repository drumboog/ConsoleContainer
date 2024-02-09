using ConsoleContainer.ProcessManagement.Events;

namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessManager<TKey>
    {
        event EventHandler<ProcessAddedEventArgs<TKey>>? ProcessAdded;
        event EventHandler<ProcessRemovedEventArgs<TKey>>? ProcessRemoved;

        IEnumerable<IProcessWrapper<TKey>> GetProcesses();
        IProcessWrapper<TKey>? GetProcess(TKey key);
        Task<IProcessWrapper<TKey>> CreateProcessAsync(TKey key, ProcessDetails processDetails);
        Task<bool> DeleteProcessAsync(TKey key);
    }
}
