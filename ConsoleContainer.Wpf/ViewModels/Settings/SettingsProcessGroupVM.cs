using ConsoleContainer.Wpf.Domain.Contracts;
using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels.Settings
{
    internal class SettingsProcessGroupVM : ViewModel, IProcessGroup
    {
        public string? GroupName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public ObservableCollection<SettingsProcessInformationVM> Processes { get; } = new();

        IEnumerable<IProcessInformation> IProcessGroup.Processes => Processes;
    }
}
