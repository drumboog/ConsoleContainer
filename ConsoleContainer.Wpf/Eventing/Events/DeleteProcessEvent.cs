using ConsoleContainer.Wpf.ViewModels;

namespace ConsoleContainer.Wpf.Eventing.Events
{
    public class DeleteProcessEvent
    {
        public ProcessVM Process { get; }

        public DeleteProcessEvent(ProcessVM process)
        {
            Process = process;
        }
    }
}
