using ConsoleContainer.Wpf.Domain.Contracts;

namespace ConsoleContainer.Wpf.ViewModels.Settings
{
    internal class SettingsProcessInformationVM : ViewModel, IProcessInformation
    {
        public string? ProcessName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

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
