using ConsoleContainer.ProcessManagement;
using ConsoleContainer.ProcessManagement.Events;
using ConsoleContainer.Wpf.ViewModels;
using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.DesignData
{
    internal class ViewModelLocator
    {
        public static ProcessContainerVM ProcessContainer { get; } = CreateProcessContainer();

        public static ProcessGroupVM ProcessGroup { get; } = ProcessContainer.ProcessGroups.First();

        public static ProcessVM Process { get; } = CreateProcessVM("Application UI", @"C:\Application\FE\ApplicationFE.exe", @"", @"C:\Application\FE\", 12345, ProcessState.Running);


        private static ProcessContainerVM CreateProcessContainer()
        {
            var result = new ProcessContainerVM();
            result.ProcessGroups.Add(
                CreateProcessGroup(
                    "Application Processes",
                    CreateProcessVM("Application UI", @"C:\Application\FE\ApplicationFE.exe", @"", @"C:\Application\FE\", 15434, ProcessState.Running),
                    CreateProcessVM("Application API", @"C:\Application\API\ApiService.exe", @"", @"C:\Application\API\", 65493, ProcessState.Idle),
                    CreateProcessVM("Application BFF", @"C:\Application\BFF\BffService.exe", @"", @"C:\Application\BFF\", 93284, ProcessState.Starting),
                    CreateProcessVM("Application Database", @"C:\Application\DB\DatabaseService.exe", @"", @"C:\Application\DB\", 20938, ProcessState.Stopping)
                )
            );
            result.ProcessGroups.Add(
                CreateProcessGroup(
                    "Windows Processes"
                )
            );
            result.ProcessGroups.Add(
                CreateProcessGroup(
                    "Other Processes"
                )
            );
            return result;
        }

        private static ProcessGroupVM CreateProcessGroup(string groupName, params ProcessVM[] processes)
        {
            var result = new ProcessGroupVM();
            result.GroupName = groupName;
            foreach (var p in processes)
            {
                result.Processes.Add(p);
            }
            return result;
        }

        private static ProcessVM CreateProcessVM(string displayName, string filePath, string arguments, string workingDirectory, int pid, ProcessState state)
        {
            return new ProcessVM(
                displayName,
                new MockProcessWrapper()
                {
                    ProcessDetails = new ProcessDetails(Guid.NewGuid(), filePath, arguments, workingDirectory),
                    ProcessId = pid,
                    State = state
                }
            );
        }


        private class MockProcessWrapper : IProcessWrapper
        {
            public event EventHandler<ProcessOutputDataEventArgs>? OutputDataReceived;
            public event EventHandler<ProcessStateChangedEventArgs>? StateChanged;

            public int? ProcessId { get; set; }

            public Guid ProcessLocator { get; } = Guid.NewGuid();

            public ProcessDetails ProcessDetails { get; set; } = new ProcessDetails(Guid.NewGuid(), string.Empty);

            public ProcessState State { get; set; }

            public List<ProcessOutputData> OutputData = new List<ProcessOutputData>();
            IReadOnlyCollection<ProcessOutputData> IProcessWrapper.OutputData => OutputData;

            public Task StartProcessAsync()
            {
                return Task.CompletedTask;
            }

            public Task StopProcessAsync()
            {
                return Task.CompletedTask;
            }

            public Task UpdateProcessDetails(ProcessDetails processDetails)
            {
                return Task.CompletedTask;
            }
        }
    }
}
