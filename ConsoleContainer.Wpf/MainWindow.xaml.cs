using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.ViewModels;
using System.Windows;

namespace ConsoleContainer.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHandle<ClosingSettingsEvent>
    {
        private ProcessContainerVM viewModel = new ProcessContainerVM();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = viewModel;

            EventAggregator.Instance.SubscribeOnUIThread(this);
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
    }
}