using ConsoleContainer.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ProcessGroupControl.xaml
    /// </summary>
    public partial class ProcessGroupControl : UserControl
    {
        public static DependencyProperty ProcessContainerProperty = DependencyProperty.Register("ProcessContainer", typeof(ProcessContainerVM), typeof(ProcessGroupControl));
        public static DependencyProperty ProcessGroupProperty = DependencyProperty.Register("ProcessGroup", typeof(ProcessGroupVM), typeof(ProcessGroupControl));

        public ProcessContainerVM ProcessContainer
        {
            get { return (ProcessContainerVM)GetValue(ProcessContainerProperty); }
            set { SetValue(ProcessContainerProperty, value); }
        }

        public ProcessGroupVM ProcessGroup
        {
            get { return (ProcessGroupVM)GetValue(ProcessGroupProperty); }
            set { SetValue(ProcessGroupProperty, value); }
        }

        public ProcessGroupControl()
        {
            InitializeComponent();
        }

        private void StartAll_Click(object sender, RoutedEventArgs e)
        {
            _ = ProcessGroup.StartAllAsync();
        }

        private void StopAll_Click(object sender, RoutedEventArgs e)
        {
            _ = ProcessGroup.StopAllAsync();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            _ = ProcessGroup.ClearAllAsync();
        }

        private void CreateProcess_Click(object sender, RoutedEventArgs e)
        {
            _ = ProcessContainer.CreateProcessAsync(ProcessGroup);
        }
    }
}
