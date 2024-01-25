using ConsoleContainer.Wpf.Controls.Dialogs;
using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHandle<ClosingSettingsEvent>, IHandle<ShowDialogEvent>
    {
        private readonly ProcessContainerVM viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ProcessContainerVM();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                viewModel.RefreshProcesses();
                DataContext = viewModel;

                EventAggregator.Instance.SubscribeOnUIThread(this);
            }
        }

        private void miSettings_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ShowSettings = true;
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public Task HandleAsync(ClosingSettingsEvent message, CancellationToken cancellationToken)
        {
            viewModel.ShowSettings = false;
            return Task.CompletedTask;
        }

        public Task HandleAsync(ShowDialogEvent message, CancellationToken cancellationToken)
        {
            var closeActions = new List<Action>();
            EventHandler closedHandler = (object? sender, EventArgs e) =>
            {
                closeActions.ForEach(a => a());
            };
            closeActions.Add(() => message.DialogClosed -= closedHandler);
            closeActions.Add(() => viewModel.Dialog = null);
            message.DialogClosed += closedHandler;
            viewModel.Dialog = message.Control;
            return Task.CompletedTask;
        }

        private void EditProcessGroup_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is ProcessGroupVM group)
            {
                _ = viewModel.EditProcessGroupAsync(group);
            }
        }

        private void DeleteProcessGroup_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is ProcessGroupVM group)
            {
                _ = viewModel.DeleteProcessGroupAsync(group);
            }
        }

        private void CreateProcessGroup_Click(object sender, RoutedEventArgs e)
        {
            _ = viewModel.CreateProcessGroupAsync();
        }
    }
}
