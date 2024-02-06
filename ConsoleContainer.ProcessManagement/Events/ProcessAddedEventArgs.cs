namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessAddedEventArgs
    {
        public IProcessWrapper Process { get; }

        public ProcessAddedEventArgs(IProcessWrapper process)
        {
            Process = process;
        }
    }
}
