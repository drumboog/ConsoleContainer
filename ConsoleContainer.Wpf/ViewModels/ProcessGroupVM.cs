using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessGroupVM : ViewModel
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

        public ObservableCollection<ProcessVM> Processes { get; } = new();

        public ProcessGroupVM()
        {
            ViewTypes = ProcessGroupViewType.ViewTypes.ToList();
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
    }
}
