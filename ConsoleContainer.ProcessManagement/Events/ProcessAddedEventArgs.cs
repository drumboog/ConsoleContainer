namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessAddedEventArgs<TKey>
    {
        public IProcessWrapper<TKey> Process { get; }

        public ProcessAddedEventArgs(IProcessWrapper<TKey> process)
        {
            Process = process;
        }
    }
}
