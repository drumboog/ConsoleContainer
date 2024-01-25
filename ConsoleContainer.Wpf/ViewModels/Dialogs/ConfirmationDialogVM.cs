namespace ConsoleContainer.Wpf.ViewModels.Dialogs
{
    public class ConfirmationDialogVM : ViewModel
    {
        public string Header { get; }
        public string Message { get; }
        public string AcceptButtonText { get; }

        public ConfirmationDialogVM(string header, string message, string? acceptButtonText = null)
        {
            Header = header;
            Message = message;
            AcceptButtonText = acceptButtonText ?? "Ok";
        }
    }
}
