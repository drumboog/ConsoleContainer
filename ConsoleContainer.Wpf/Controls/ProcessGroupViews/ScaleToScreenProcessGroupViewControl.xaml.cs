﻿using ConsoleContainer.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
