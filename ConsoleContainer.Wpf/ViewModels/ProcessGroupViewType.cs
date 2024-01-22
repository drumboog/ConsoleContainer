using ConsoleContainer.Wpf.Controls.ProcessGroupViews;
using System.Windows.Controls;

namespace ConsoleContainer.Wpf.ViewModels
{
    public class ProcessGroupViewType
    {
        public string DisplayName { get; }
        public Control Control { get; }

        public static IEnumerable<ProcessGroupViewType> ViewTypes { get; } = [
            new("Scale to Screen", new ScaleToScreenProcessGroupViewControl()),
            new("Tabs", new TabbedProcessGroupViewControl()),
            new("List", new GridProcessGroupViewControl() { ColumnCount = 1 }),
            new("2 Columns", new GridProcessGroupViewControl() { ColumnCount = 2 }),
            new("3 Columns", new GridProcessGroupViewControl() { ColumnCount = 3 }),
            new("4 Columns", new GridProcessGroupViewControl() { ColumnCount = 4 }),
            new("5 Columns", new GridProcessGroupViewControl() { ColumnCount = 5 }),
            new("6 Columns", new GridProcessGroupViewControl() { ColumnCount = 6 }),
        ];

        private ProcessGroupViewType(string displayName, Control control)
        {
            DisplayName = displayName;
            Control = control;
        }
    }
}
