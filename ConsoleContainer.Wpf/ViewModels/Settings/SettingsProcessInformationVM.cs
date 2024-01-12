using ConsoleContainer.Wpf.Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ConsoleContainer.Wpf.ViewModels.Settings
{
    internal class SettingsProcessInformationVM : ViewModel, IProcessInformation
    {
        [Required]
        public string? ProcessName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        [Required]
        public string? FileName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public string? Arguments
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public string? WorkingDirectory
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
