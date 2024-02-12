﻿using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using System.Diagnostics;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessVM : ViewModel
    {
        private readonly IWorkerServiceClient workerServiceClient;
        private readonly IEventAggregator eventAggregator;

        public Guid ProcessGroupId { get; }

        public Guid ProcessLocator { get; }

        public int? ProcessId
        {
            get => GetProperty<int?>();
            private set => SetProperty(value);
        }

        public string ProcessName
        {
            get => GetProperty(() => string.Empty);
            private set => SetProperty(value);
        }

        public string FilePath
        {
            get => GetProperty(() => string.Empty);
            private set => SetProperty(value);
        }

        public string? Arguments
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        public string? WorkingDirectory
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        public ProcessState State
        {
            get => GetProperty(ProcessState.Idle);
            private set => SetProperty(value, [nameof(IsRunning), nameof(CanStart), nameof(CanStop)]);
        }

        public ProcessOutputVM Output { get; } = new ProcessOutputVM();

        public bool IsRunning => State is ProcessState.Starting or ProcessState.Running or ProcessState.Stopping;

        public bool CanStart => !IsRunning;

        public bool CanStop => IsRunning;

        public ProcessVM(
            IWorkerServiceClient workerServiceClient,
            IEventAggregator eventAggregator,
            Guid processGroupId,
            Guid processLocator,
            int? processId,
            string processName,
            string filePath,
            string? arguments,
            string? workingDirectory,
            ProcessState state)
        {
            this.workerServiceClient = workerServiceClient;
            this.eventAggregator = eventAggregator;
            ProcessGroupId = processGroupId;
            ProcessLocator = processLocator;
            Update(processName, filePath, arguments, workingDirectory);
            UpdateState(state, processId);
        }

        public void Update(string processName, string filePath, string? arguments, string? workingDirectory)
        {
            ProcessName = processName;
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }

        public void UpdateState(ProcessState state, int? processId)
        {
            State = state;
            ProcessId = processId;
        }

        public async Task StartProcessAsync()
        {
            if (IsRunning)
            {
                return;
            }

            await workerServiceClient.StartProcessAsync(ProcessGroupId, ProcessLocator);
        }

        public async Task StopProcessAsync()
        {
            await workerServiceClient.StopProcessAsync(ProcessGroupId, ProcessLocator);
        }

        public Task ClearOutputAsync()
        {
            Output.ClearLogs();
            return Task.CompletedTask;
        }
    }
}
