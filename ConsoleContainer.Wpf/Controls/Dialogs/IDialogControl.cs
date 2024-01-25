namespace ConsoleContainer.Wpf.Controls.Dialogs
{
    public class DialogClosedEventArgs<T>
    {
        public T CloseContext { get; }

        public DialogClosedEventArgs(T closeContext)
        {
            CloseContext = closeContext;
        }
    }

    internal interface IDialogControl<T>
    {
        event EventHandler<DialogClosedEventArgs<T>>? DialogClosed;
    }
}
