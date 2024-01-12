using ConsoleContainer.Wpf.Eventing;
using ConsoleContainer.Wpf.Eventing.Events;
using ConsoleContainer.Wpf.ViewModels.Settings;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private readonly SettingsVM viewModel = new SettingsVM();

        public SettingsControl()
        {
            InitializeComponent();

            for (var i = 1; i < 10; i++)
            {
                var group = new SettingsProcessGroupVM()
                {
                    GroupName = $"Group {i}"
                };
                for (var j = 1; j < 5; j++)
                {
                    group.Processes.Add(new SettingsProcessInformationVM()
                    {
                        ProcessName = $"Test Process {j}",
                        FileName = $"Test File {j}",
                        Arguments = $"Test Arguments {j}",
                        WorkingDirectory = $"Test Working Directory {j}"
                    });
                }
                viewModel.ProcessGroups.Add(group);
            }
            DataContext = viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _ = EventAggregator.Instance.PublishOnCurrentThreadAsync(new ClosingSettingsEvent());
        }

        private void MoveProcessGroupUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.CommandParameter is not SettingsProcessGroupVM group)
            {
                return;
            }

            viewModel.MoveGroupUp(group);
        }

        private void MoveProcessGroupDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.CommandParameter is not SettingsProcessGroupVM group)
            {
                return;
            }

            viewModel.MoveGroupDown(group);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ = EventAggregator.Instance.PublishOnCurrentThreadAsync(new ClosingSettingsEvent());
        }
    }
}
