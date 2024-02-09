namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessStateChangedEventArgs<TKey>
    {
        public TKey ProcessKey { get; }
        public ProcessState State { get; }
        public int? ProcessId { get; }

        public ProcessStateChangedEventArgs(TKey processKey, ProcessState state, int? processId)
        {
            ProcessKey = processKey;
            State = state;
            ProcessId = processId;
        }
    }
}
