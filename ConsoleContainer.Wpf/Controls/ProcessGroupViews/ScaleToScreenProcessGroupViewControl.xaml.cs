using ConsoleContainer.Wpf.Domain;
using ConsoleContainer.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConsoleContainer.Wpf.Controls.ProcessGroupViews
{
    /// <summary>
    /// Interaction logic for ScaleToScreenProcessGroupViewControl.xaml
    /// </summary>
    public partial class ScaleToScreenProcessGroupViewControl : UserControl
    {
        public ScaleToScreenProcessGroupViewControl()
        {
            InitializeComponent();
        }

        private void UniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UniformGrid grid)
            {
                UpdateGrid(grid);
            }
        }

        private void UniformGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UniformGrid grid)
            {
                UpdateGrid(grid);
            }
        }

        private void UpdateGrid(UniformGrid grid)
        {
            if (!(grid.DataContext is ProcessGroupVM processGroup))
            {
                return;
            }

            var sqrt = Math.Sqrt(processGroup.Processes.Count);
            int count = (int)Math.Floor(sqrt);
            if (count != sqrt)
            {
                count++;
            }
            grid.Columns = count;
        }
    }
}
