using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class ShowDialogEvent
    {
        public UserControl Control { get; }
        public event EventHandler? DialogClosed;

        public ShowDialogEvent(UserControl control)
        {
            Control = control;
        }

        public void NotifyDialogClosed()
        {
            DialogClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
