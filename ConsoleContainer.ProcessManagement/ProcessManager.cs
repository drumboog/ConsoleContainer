﻿using ConsoleContainer.ProcessManagement.Events;
using System.Collections.Concurrent;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessManager : IProcessManager
    {
        private readonly ConcurrentDictionary<Guid, IProcessWrapper> processes = new();
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

        public IProcessWrapper? GetProcess(Guid processLocator)
        {
            processes.TryGetValue(processLocator, out var p);
            return p;
        }

        public Task<IProcessWrapper> CreateProcessAsync(ProcessDetails processDetails)
        {
            var process = processes.AddOrUpdate(
                processDetails.ProcessLocator,
                locator => processWrapperFactory.CreateProcessWrapper(processDetails),
                (locator, pw) => throw new Exception($"Process already exists with process locator {locator}")
            );

            OnProcessAdded(new ProcessAddedEventArgs(process));

            return Task.FromResult(process);
        }

        public Task UpdateProcessAsync(ProcessDetails processDetails)
        {
            var process = processes.AddOrUpdate(
                processDetails.ProcessLocator,
                locator => throw new Exception($"Process does not exist with process locator {locator}"),
                (locator, pw) =>
                {
                    pw.UpdateProcessDetails(processDetails);
                    return pw;
                }
            );

            return Task.CompletedTask;
        }

        public async Task<bool> DeleteProcessAsync(Guid processLocator)
        {
            if (!processes.Remove(processLocator, out var process))
            {
                return false;
            }

            await process.StopProcessAsync();

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