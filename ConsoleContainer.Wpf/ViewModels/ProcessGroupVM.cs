using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessGroupVM : ViewModel
    {
        public ObservableCollection<ProcessVM> Processes { get; } = new();
    }
}
