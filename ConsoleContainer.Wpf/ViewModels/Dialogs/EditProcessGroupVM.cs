namespace ConsoleContainer.Wpf.ViewModels.Dialogs
{
    public class EditProcessGroupVM : ViewModel
    {
        public string GroupName
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }
    }
}
