using ConsoleContainer.Contracts;
using ConsoleContainer.WorkerService.Client;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.Services;
using ConsoleContainer.Wpf.ViewModels.Dialogs;
using ConsoleContainer.Wpf.ViewModels.Factories;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessContainerVM : ViewModel,
        IHandle<EditProcessEvent>,
        IHandle<DeleteProcessEvent>,
        IHandle<ProcessGroupCreatedEvent>,
        IHandle<ProcessGroupUpdatedEvent>
    {
        private readonly IDialogService dialogService;
        private readonly IWorkerServiceClient workerServiceClient;
        private readonly IProcessVmFactory processVmFactory;

        public ObservableCollection<ProcessGroupVM> ProcessGroups { get; } = new();

        public Control? Dialog
        {
            get => GetProperty<Control>();
            set => SetProperty(value);
        }

        public ProcessContainerVM(
            IDialogService dialogService,
            IWorkerServiceClient workerServiceClient,
            IProcessVmFactory processVmFactory,
            IEventAggregator eventAggregator
        )
        {
            this.dialogService = dialogService;
            this.workerServiceClient = workerServiceClient;
            this.processVmFactory = processVmFactory;

            eventAggregator.SubscribeOnUIThread(this);
        }

        public void AddProcessGroup(ProcessGroupVM processGroup)
        {
            ProcessGroups.Add(processGroup);
        }

        public async Task CreateProcessGroupAsync()
        {
            var newGroup = await dialogService.CreateProcessGroupAsync();
            if (newGroup is null)
            {
                return;
            }

            await workerServiceClient.CreateProcessGroupAsync(new ProcessGroupDto() { ProcessGroupId = Guid.NewGuid(), GroupName = newGroup.GroupName });
        }

        public async Task EditProcessGroupAsync(ProcessGroupVM processGroup)
        {
            var vm = new EditProcessGroupVM() { GroupName = processGroup.GroupName };
            if (!await dialogService.EditProcessGroupAsync(vm))
            {
                return;
            }

            await workerServiceClient.UpdateProcessGroupAsync(processGroup.ProcessGroupId, new ProcessGroupUpdateDto() { GroupName = vm.GroupName });
        }

        public async Task DeleteProcessGroupAsync(ProcessGroupVM processGroup)
        {
            var result = await dialogService.ShowConfirmationDialogAsync(new ConfirmationDialogVM("Delete Process Group?", $"Are you sure you want to delete the process group: {processGroup.GroupName}?"));
            if (!result)
            {
                return;
            }

            ProcessGroups.Remove(processGroup);
        }

        public async Task CreateProcessAsync(ProcessGroupVM processGroup)
        {
            var result = await dialogService.CreateProcessAsync();
            if (result is null)
            {
                return;
            }

            await workerServiceClient.CreateProcessAsync(
                processGroup.ProcessGroupId,
                new ProcessInformationDto()
                {
                    ProcessLocator = Guid.NewGuid(),
                    ProcessName = result.ProcessName,
                    FilePath = result.FilePath,
                    Arguments = result.Arguments,
                    WorkingDirectory = result.WorkingDirectory
                });
        }

        public async Task RefreshProcessesAsync()
        {
            var runningProcesses = ProcessGroups.SelectMany(x => x.Processes).Where(x => x.IsRunning);
            if (runningProcesses.Any())
            {
                throw new Exception("Cannot refresh when processes are running");
            }

            var savedProcessGroups = await workerServiceClient.GetProcessGroupsAsync(CancellationToken.None);

            var newProcessGroups = new List<ProcessGroupVM>();
            foreach (var processGroup in savedProcessGroups)
            {
                newProcessGroups.Add(CreateProcessGroup(processGroup));
            }

            ProcessGroups.Clear();
            newProcessGroups.ForEach(x => ProcessGroups.Add(x));
        }

        private ProcessGroupVM CreateProcessGroup(ProcessGroupSummaryDto processGroup)
        {
            var vm = new ProcessGroupVM(processGroup.ProcessGroupId, processGroup.GroupName);
            foreach(var p in processGroup.Processes)
            {
                var newProcess = processVmFactory.Create(processGroup.ProcessGroupId, p.ProcessLocator, p.ProcessId, p.ProcessName ?? string.Empty, p.FilePath ?? string.Empty, p.Arguments, p.WorkingDirectory, p.State);
                vm.Processes.Add(newProcess);
            }

            return vm;
        }

        public async Task HandleAsync(EditProcessEvent message, CancellationToken cancellationToken)
        {
            var process = message.Process;
            var processGroup = ProcessGroups.FirstOrDefault(g => g.Processes.Any(p => p.ProcessLocator == process.ProcessLocator));
            if (processGroup is null)
            {
                return;
            }

            var vm = new EditProcessVM()
            {
                ProcessName = process.ProcessName,
                FilePath = process.FilePath,
                Arguments = process.Arguments,
                WorkingDirectory = process.WorkingDirectory
            };

            if (!await dialogService.EditProcessAsync(vm))
            {
                return;
            }

            await workerServiceClient.UpdateProcessAsync(
                processGroup.ProcessGroupId,
                process.ProcessLocator,
                new ProcessInformationUpdateDto()
                {
                    ProcessName = vm.ProcessName,
                    FilePath = vm.FilePath,
                    Arguments = vm.Arguments,
                    WorkingDirectory = vm.WorkingDirectory
                });
        }

        public async Task HandleAsync(DeleteProcessEvent message, CancellationToken cancellationToken)
        {
            var process = message.Process;
            var result = await dialogService.ShowConfirmationDialogAsync(new ConfirmationDialogVM("Delete Process?", $"Are you sure you want to delete the process: {process.ProcessName}?"));
            if (!result)
            {
                return;
            }

            foreach (var group in ProcessGroups)
            {
                group.Processes.Remove(process);
            }
        }

        public Task HandleAsync(ProcessGroupCreatedEvent message, CancellationToken cancellationToken)
        {
            var group = message.ProcessGroup;
            ProcessGroups.Add(new ProcessGroupVM(group.ProcessGroupId, group.GroupName));
            return Task.CompletedTask;
        }

        public Task HandleAsync(ProcessGroupUpdatedEvent message, CancellationToken cancellationToken)
        {
            var updatedGroup = message.ProcessGroup;
            var group = ProcessGroups.FirstOrDefault(p => p.ProcessGroupId == updatedGroup.ProcessGroupId);
            if (group is null)
            {
                return Task.CompletedTask;
            }
            group.Update(updatedGroup.GroupName);
            return Task.CompletedTask;
        }
    }
}
