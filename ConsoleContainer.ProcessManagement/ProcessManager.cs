using ConsoleContainer.ProcessManagement.Events;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessManager<TKey> : IProcessManager<TKey>
        where TKey: notnull
    {
        private readonly ConcurrentDictionary<TKey, IProcessWrapper<TKey>> processes = new();
        private readonly IProcessWrapperFactory<TKey> processWrapperFactory;

        public event EventHandler<ProcessAddedEventArgs<TKey>>? ProcessAdded;
        public event EventHandler<ProcessRemovedEventArgs<TKey>>? ProcessRemoved;

        public ProcessManager()
        {
            processWrapperFactory = new ProcessWrapperFactory<TKey>(NullLogger<ProcessWrapper<TKey>>.Instance);
        }

        public ProcessManager(IProcessWrapperFactory<TKey> processWrapperFactory)
        {
            this.processWrapperFactory = processWrapperFactory;
        }

        public IEnumerable<IProcessWrapper<TKey>> GetProcesses()
        {
            return processes.Values;
        }

        public IProcessWrapper<TKey>? GetProcess(TKey key)
        {
            processes.TryGetValue(key, out var p);
            return p;
        }

        public Task<IProcessWrapper<TKey>> CreateProcessAsync(TKey key, ProcessDetails processDetails)
        {
            var process = processes.AddOrUpdate(
                key,
                locator => processWrapperFactory.CreateProcessWrapper(key, processDetails),
                (locator, pw) => throw new Exception($"Process already exists with process locator {locator}")
            );

            OnProcessAdded(new ProcessAddedEventArgs<TKey>(process));

            return Task.FromResult(process);
        }

        public Task UpdateProcessAsync(TKey key, ProcessDetails processDetails)
        {
            var process = processes.AddOrUpdate(
                key,
                locator => throw new Exception($"Process does not exist with process locator {locator}"),
                (locator, pw) =>
                {
                    pw.UpdateProcessDetails(processDetails);
                    return pw;
                }
            );

            return Task.CompletedTask;
        }

        public async Task<bool> DeleteProcessAsync(TKey key)
        {
            if (!processes.Remove(key, out var process))
            {
                return false;
            }

            await process.StopProcessAsync();

            OnProcessRemoved(new ProcessRemovedEventArgs<TKey>(process));

            return true;
        }

        protected virtual void OnProcessAdded(ProcessAddedEventArgs<TKey> e)
        {
            ProcessAdded?.Invoke(this, e);
        }

        protected virtual void OnProcessRemoved(ProcessRemovedEventArgs<TKey> e)
        {
            ProcessRemoved?.Invoke(this, e);
        }
    }
}
