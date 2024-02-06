using ConsoleContainer.ProcessManagement.Events;
using System.Collections.Concurrent;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessManager : IProcessManager
    {
        private readonly ConcurrentDictionary<string, IProcessWrapper> processes = new();
        private readonly IProcessWrapperFactory processWrapperFactory;

        public event EventHandler<ProcessAddedEventArgs>? ProcessAdded;
        public event EventHandler<ProcessRemovedEventArgs>? ProcessRemoved;

        public ProcessManager()
        {
            processWrapperFactory = new ProcessWrapperFactory();
        }

        public ProcessManager(IProcessWrapperFactory processWrapperFactory)
        {
            this.processWrapperFactory = processWrapperFactory;
        }

        public IEnumerable<IProcessWrapper> GetProcesses()
        {
            return processes.Values;
        }

        public IProcessWrapper? GetProcess(string processLocator)
        {
            processes.TryGetValue(processLocator, out var p);
            return p;
        }

        public IProcessWrapper CreateProcess(ProcessDetails processDetails)
        {
            var process = processes.AddOrUpdate(
                processDetails.ProcessLocator,
                locator => processWrapperFactory.CreateProcessWrapper(processDetails),
                (locator, pw) => throw new Exception($"Process already exists with process locator {locator}")
            );

            OnProcessAdded(new ProcessAddedEventArgs(process));

            return process;
        }

        public bool RemoveProcess(string processLocator)
        {
            if (!processes.Remove(processLocator, out var process))
            {
                return false;
            }

            process.StopProcess();

            OnProcessRemoved(new ProcessRemovedEventArgs(process));

            return true;
        }

        protected virtual void OnProcessAdded(ProcessAddedEventArgs e)
        {
            ProcessAdded?.Invoke(this, e);
        }

        protected virtual void OnProcessRemoved(ProcessRemovedEventArgs e)
        {
            ProcessRemoved?.Invoke(this, e);
        }
    }
}
