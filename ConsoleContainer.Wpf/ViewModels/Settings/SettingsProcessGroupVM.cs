using ConsoleContainer.Wpf.Domain.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleContainer.Wpf.ViewModels.Settings
{
    internal class SettingsProcessGroupVM : ViewModel, IProcessGroup
    {
        [Required]
        public string? GroupName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public ObservableCollection<SettingsProcessInformationVM> Processes { get; } = new();

        IEnumerable<IProcessInformation> IProcessGroup.Processes => Processes;

        public void AddProcess()
        {
            Processes.Add(new SettingsProcessInformationVM());
        }

        public void RemoveProcess(SettingsProcessInformationVM process)
        {
            Processes.Remove(process);
        }
    }
}
