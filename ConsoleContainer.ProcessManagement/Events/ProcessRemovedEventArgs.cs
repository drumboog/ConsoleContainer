namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessRemovedEventArgs
    {
        public IProcessWrapper Process { get; }

        public ProcessRemovedEventArgs(IProcessWrapper process)
        {
            Process = process;
        }
    }
}
