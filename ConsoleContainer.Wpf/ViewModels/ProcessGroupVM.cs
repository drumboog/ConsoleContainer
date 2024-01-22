using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessGroupVM : ViewModel
    {
        public string GroupName
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
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

        public ProcessGroupVM()
        {
            ViewTypes = ProcessGroupViewType.ViewTypes.ToList();

            Processes.CollectionChanged += Processes_CollectionChanged;
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

        public void StartAll()
        {
            foreach (ProcessVM process in Processes)
            {
                process.StartProcess();
            }
        }

        public void StopAll()
        {
            foreach (ProcessVM process in Processes)
            {
                process.StopProcess();
            }
        }

        public void ClearAll()
        {
            foreach (ProcessVM process in Processes)
            {
                process.ClearOutput();
            }
        }

        private void Process_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProcessVM.IsRunning))
            {
                OnPropertyChanged(nameof(RunningProcesses));
            }
        }
    }
}
