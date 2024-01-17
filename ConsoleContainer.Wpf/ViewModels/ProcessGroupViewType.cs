using System.Collections.ObjectModel;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessGroupViewType
    {
        public string DisplayName { get; }

        public static IEnumerable<ProcessGroupViewType> ViewTypes { get; } = [
            new("Tabs"),
            new("List"),
            new("Grid - 2 Columns"),
            new("Grid - 3 Columns"),
            new("Grid - 4 Columns")
        ];

        private ProcessGroupViewType(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
