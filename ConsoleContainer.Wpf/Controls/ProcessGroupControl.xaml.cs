using ConsoleContainer.Wpf.ViewModels;
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

namespace ConsoleContainer.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ProcessGroupControl.xaml
    /// </summary>
    public partial class ProcessGroupControl : UserControl
    {
        public static DependencyProperty ProcessGroupProperty = DependencyProperty.Register("ProcessGroup", typeof(ProcessGroupVM), typeof(ProcessGroupControl));

        internal ProcessGroupVM ProcessGroup
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
            ProcessGroup.StartAll();
        }

        private void StopAll_Click(object sender, RoutedEventArgs e)
        {
            ProcessGroup.StopAll();
        }
    }
}
