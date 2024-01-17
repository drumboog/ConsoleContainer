using ConsoleContainer.Wpf.Controls;
using System.Windows.Media;

namespace ConsoleContainer.Wpf.ViewModels
{
    internal class ProcessOutputVM : ViewModel
    {
        public ConsoleLogControl ConsoleLog { get; } = new();

        public void AddOutput(string? message, Brush? foreground = null)
        {
            ConsoleLog.AddOutput(message, foreground);
        }

        public void ClearLogs()
        {
            ConsoleLog.ClearLogs();
        }
    }
}
