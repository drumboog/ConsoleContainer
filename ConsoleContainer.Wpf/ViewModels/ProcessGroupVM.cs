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

        public ObservableCollection<ProcessVM> Processes { get; } = new();
    }
}
