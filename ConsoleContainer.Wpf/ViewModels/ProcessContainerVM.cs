using ConsoleContainer.Contracts;
using ConsoleContainer.Domain;
using ConsoleContainer.Repositories;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.Services;
using ConsoleContainer.Wpf.ViewModels.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessContainerVM : ViewModel, IHandle<EditProcessEvent>, IHandle<DeleteProcessEvent>
    {
        private IDialogService dialogService;

        public ObservableCollection<ProcessGroupVM> ProcessGroups { get; } = new();

        public Control? Dialog
        {
            get => GetProperty<Control>();
            set => SetProperty(value);
        }

        public ProcessContainerVM()
        {
            dialogService = DialogService.Instance;

            App.EventAggregator.SubscribeOnUIThread(this);
        }

        public async Task CreateProcessGroupAsync()
        {
            var newGroup = await dialogService.CreateProcessGroupAsync();
            if (newGroup is null)
            {
                return;
            }

            ProcessGroups.Add(new ProcessGroupVM() { GroupName = newGroup.GroupName });
            await SaveAsync();
        }

        public async Task EditProcessGroupAsync(ProcessGroupVM processGroup)
        {
            var vm = new EditProcessGroupVM() { GroupName = processGroup.GroupName };
            if (!await dialogService.EditProcessGroupAsync(vm))
            {
                return;
            }

            processGroup.GroupName = vm.GroupName;
            await SaveAsync();
        }

        public async Task DeleteProcessGroupAsync(ProcessGroupVM processGroup)
        {
            var result = await dialogService.ShowConfirmationDialogAsync(new ConfirmationDialogVM("Delete Process Group?", $"Are you sure you want to delete the process group: {processGroup.GroupName}?"));
            if (!result)
            {
                return;
            }

            ProcessGroups.Remove(processGroup);
            await SaveAsync();
        }

        public async Task CreateProcessAsync(ProcessGroupVM processGroup)
        {
            var result = await dialogService.CreateProcessAsync();
            if (result is null)
            {
                return;
            }

            processGroup.AddProcess(new ProcessInformation(Guid.NewGuid(), result.ProcessName, result.FilePath, result.Arguments, result.WorkingDirectory));
            await SaveAsync();

            //await App.ProcessHub.AddProcessAsync(new NewProcessDto()
            //{
            //    ProcessLocator = Guid.NewGuid(),
            //    FilePath = result.FilePath,
            //    Arguments = result.Arguments,
            //    WorkingDirectory = result.WorkingDirectory
            //});
        }

        public async Task RefreshProcessesAsync()
        {
            var runningProcesses = ProcessGroups.SelectMany(x => x.Processes).Where(x => x.IsRunning);
            if (runningProcesses.Any())
            {
                throw new Exception("Cannot refresh when processes are running");
            }

            var repo = new ProcessGroupCollectionRepository();
            var processGroups = await repo.ReadAsync();

            var newProcessGroups = new List<ProcessGroupVM>();
            foreach (var processGroup in processGroups.ProcessGroups)
            {
                newProcessGroups.Add(CreateProcessGroup(processGroup));
            }

            ProcessGroups.Clear();
            newProcessGroups.ForEach(x => ProcessGroups.Add(x));
        }

        private ProcessGroupVM CreateProcessGroup(ProcessGroup processGroup)
        {
            var vm = new ProcessGroupVM();
            vm.GroupName = processGroup.GroupName;
            foreach(var p in processGroup.Processes)
            {
                vm.Processes.Add(new ProcessVM(p));
            }

            return vm;
        }

        public async Task HandleAsync(EditProcessEvent message, CancellationToken cancellationToken)
        {
            var process = message.Process;
            var vm = new EditProcessVM()
            {
                ProcessName = process.ProcessInformation.ProcessName,
                FilePath = process.ProcessInformation.FilePath,
                Arguments = process.ProcessInformation.Arguments,
                WorkingDirectory = process.ProcessInformation.WorkingDirectory
            };
            if (!await dialogService.EditProcessAsync(vm))
            {
                return;
            }

            process.Update(new ProcessInformation(process.ProcessInformation.ProcessLocator, vm.ProcessName, vm.FilePath, vm.Arguments, vm.WorkingDirectory));
            await SaveAsync();
        }

        public async Task HandleAsync(DeleteProcessEvent message, CancellationToken cancellationToken)
        {
            var process = message.Process;
            var result = await dialogService.ShowConfirmationDialogAsync(new ConfirmationDialogVM("Delete Process?", $"Are you sure you want to delete the process: {process.DisplayName}?"));
            if (!result)
            {
                return;
            }

            foreach (var group in ProcessGroups)
            {
                group.Processes.Remove(process);
            }
            await SaveAsync();
        }

        private async Task SaveAsync()
        {
            var collection = new ProcessGroupCollection();
            foreach (var group in ProcessGroups)
            {
                var newGroup = collection.AddGroup(Guid.NewGuid(), group.GroupName);
                foreach (var process in group.Processes)
                {
                    //newGroup.AddProcess(process.ProcessInformation.ProcessLocator, process.ProcessInformation);
                }
            }

            var repo = new ProcessGroupCollectionRepository();
            await repo.SaveAsync(collection);
        }
    }
}
