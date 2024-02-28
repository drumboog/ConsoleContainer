using ConsoleContainer.WorkerService.Client;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessGroupVM : ViewModel
    {
        private readonly IWorkerServiceClient workerServiceClient;

        public Guid ProcessGroupId { get; }

        public string GroupName
        {
            get => GetProperty(() => string.Empty);
            private set => SetProperty(value);
        }

        public List<ProcessGroupViewType> ViewTypes { get; }

        public ProcessGroupViewType SelectedViewType
        {
            get => GetProperty(() => ViewTypes.First());
            set => SetProperty(value);
        }

        public int SelectedIndex
        {
            get => GetProperty(0);
            set => SetProperty(value);
        }

        public int RunningProcesses => Processes.Count(x => x.IsRunning);

        public int TotalProcesses => Processes.Count();

        public ObservableCollection<ProcessVM> Processes { get; } = new();

        public ProcessGroupVM(Guid processGroupId, string groupName, IWorkerServiceClient workerServiceClient)
        {
            this.workerServiceClient = workerServiceClient;

            ProcessGroupId = processGroupId;
            GroupName = groupName;

            ViewTypes = ProcessGroupViewType.ViewTypes.ToList();

            Processes.CollectionChanged += Processes_CollectionChanged;
        }

        public void Update(string groupName)
        {
            GroupName = groupName;
        }

        public async Task StartAllAsync()
        {
            var processLocators = Processes.Select(x => x.ProcessLocator);
            await workerServiceClient.StartProcessesAsync(ProcessGroupId, processLocators);
        }

        public async Task StopAllAsync()
        {
            var processLocators = Processes.Select(x => x.ProcessLocator);
            await workerServiceClient.StopProcessesAsync(ProcessGroupId, processLocators);
        }

        public async Task ClearAllAsync()
        {
            var tasks = Processes.Select(x => x.ClearOutputAsync());
            await Task.WhenAll(tasks);
        }

        public void AddProcess(ProcessVM process)
        {
            Processes.Add(process);
        }

        public bool DeleteProcess(Guid processLocator)
        {
            var process = Processes.FirstOrDefault(p => p.ProcessLocator == processLocator);
            if (process is null)
            {
                return false;
            }
            return Processes.Remove(process);
        }

        private void Process_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProcessVM.IsRunning))
            {
                OnPropertyChanged(nameof(RunningProcesses));
            }
        }

        private void Processes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (ProcessVM oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= Process_PropertyChanged;
                }
            }
            if (e.NewItems is not null)
            {
                foreach (ProcessVM newItem in e.NewItems)
                {
                    newItem.PropertyChanged += Process_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(TotalProcesses));
        }
    }
}
