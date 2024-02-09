namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessRemovedEventArgs<TKey>
    {
        public IProcessWrapper<TKey> Process { get; }

        public ProcessRemovedEventArgs(IProcessWrapper<TKey> process)
        {
            Process = process;
        }
    }
}
