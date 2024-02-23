using ConsoleContainer.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ProcessControl.xaml
    /// </summary>
    public partial class ProcessControl : UserControl
    {
        public static DependencyProperty ProcessProperty = DependencyProperty.Register("Process", typeof(ProcessVM), typeof(ProcessControl));

        internal ProcessVM Process
        {
            get { return (ProcessVM)GetValue(ProcessProperty); }
            set { SetValue(ProcessProperty, value); }
        }

        public ProcessControl()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.StartProcessAsync();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.StopProcessAsync();
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.ClearOutputAsync();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.EditProcessAsync();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.DeleteProcessAsync();
        }
    }
}
