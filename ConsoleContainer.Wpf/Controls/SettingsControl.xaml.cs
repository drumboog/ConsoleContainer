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

            viewModel.Load();
            DataContext = viewModel;
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

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddGroup();
        }

        private void RemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.CommandParameter is SettingsProcessGroupVM group))
            {
                return;
            }
            viewModel.RemoveGroup(group);
        }

        private void AddProcess_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.CommandParameter is SettingsProcessGroupVM group))
            {
                return;
            }
            group.AddProcess();
        }

        private void RemoveProcess_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.CommandParameter is object[] data))
            {
                return;
            }
            if (data.Length != 2 || !(data[0] is SettingsProcessGroupVM group) || !(data[1] is SettingsProcessInformationVM process))
            {
                return;
            }
            group.RemoveProcess(process);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _ = EventAggregator.Instance.PublishOnCurrentThreadAsync(new ClosingSettingsEvent());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ = EventAggregator.Instance.PublishOnCurrentThreadAsync(new ClosingSettingsEvent());
        }
    }
}
