using ConsoleContainer.Wpf.ViewModels;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class EditProcessEvent
    {
        public ProcessVM Process { get; }

        public EditProcessEvent(ProcessVM process)
        {
            Process = process;
        }
    }
}
