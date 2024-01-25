using ConsoleContainer.Wpf.ViewModels.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for EditProcessGroupControl.xaml
    /// </summary>
    public partial class EditProcessGroupControl : UserControl, IDialogControl<bool>
    {
        public event EventHandler<DialogClosedEventArgs<bool>>? DialogClosed;

        private EditProcessGroupVM? viewModel;

        public EditProcessGroupVM? ViewModel
        {
            get => viewModel;
            set => DataContext = viewModel = value;
        }

        public EditProcessGroupControl()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClosed(new DialogClosedEventArgs<bool>(false));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClosed(new DialogClosedEventArgs<bool>(true));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClosed(new DialogClosedEventArgs<bool>(false));
        }

        protected void OnDialogClosed(DialogClosedEventArgs<bool> args)
        {
            DialogClosed?.Invoke(this, args);
        }
    }
}
