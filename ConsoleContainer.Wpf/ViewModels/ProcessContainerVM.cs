using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.Repositories;
using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessContainerVM : ViewModel, IHandle<SettingsSavedEvent>
    {
        public ObservableCollection<ProcessGroupVM> ProcessGroups { get; } = new();

        public bool ShowSettings
        {
            get => GetProperty(false);
            set => SetProperty(value);
        }

        public ProcessContainerVM()
        {
            EventAggregator.Instance.SubscribeOnUIThread(this);
        }

        public void RefreshProcesses()
        {
            var runningProcesses = ProcessGroups.SelectMany(x => x.Processes).Where(x => x.IsRunning);
            if (runningProcesses.Any())
            {
                throw new Exception("Cannot refresh when processes are running");
            }

            var repo = new ProcessGroupCollectionRepository();
            var processGroups = repo.Read();

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
                vm.Processes.Add(CreateProcess(p));
            }

            return vm;
        }

        private ProcessVM CreateProcess(ProcessInformation processInformation)
        {
            return new ProcessVM(processInformation);
        }

        public Task HandleAsync(SettingsSavedEvent message, CancellationToken cancellationToken)
        {
            RefreshProcesses();
            return Task.CompletedTask;
        }
    }
}
