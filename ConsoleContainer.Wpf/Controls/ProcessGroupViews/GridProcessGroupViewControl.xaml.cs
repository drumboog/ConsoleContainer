using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for GridProcessGroupViewControl.xaml
    /// </summary>
    public partial class GridProcessGroupViewControl : UserControl
    {
        public int ColumnCount { get; set; } = 1;

        public GridProcessGroupViewControl()
        {
            InitializeComponent();
        }

        private void UniformGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is UniformGrid grid))
            {
                return;
            }

            grid.Columns = ColumnCount;
        }
    }
}
