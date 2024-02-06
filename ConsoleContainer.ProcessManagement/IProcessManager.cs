using ConsoleContainer.ProcessManagement.Events;

namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessManager
    {
        event EventHandler<ProcessAddedEventArgs>? ProcessAdded;
        event EventHandler<ProcessRemovedEventArgs>? ProcessRemoved;

        IEnumerable<IProcessWrapper> GetProcesses();
        IProcessWrapper? GetProcess(string processLocator);
        IProcessWrapper CreateProcess(ProcessDetails processDetails);
    }
}
