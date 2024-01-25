using ConsoleContainer.Wpf.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConsoleContainer.Wpf.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmationControl.xaml
    /// </summary>
    public partial class ConfirmationControl : UserControl, IDialogControl<bool>
    {
        public event EventHandler<DialogClosedEventArgs<bool>>? DialogClosed;

        private ConfirmationDialogVM? viewModel;

        public ConfirmationDialogVM? ViewModel
        {
            get => viewModel;
            set => DataContext = viewModel = value;
        }

        public ConfirmationControl()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClosed(new DialogClosedEventArgs<bool>(false));
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
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
