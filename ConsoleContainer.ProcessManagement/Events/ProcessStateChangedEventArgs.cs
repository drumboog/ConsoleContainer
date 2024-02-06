namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessStateChangedEventArgs
    {
        public ProcessState State { get; }
        public int? ProcessId { get; }

        public ProcessStateChangedEventArgs(ProcessState state, int? processId)
        {
            State = state;
            ProcessId = processId;
        }
    }
}
