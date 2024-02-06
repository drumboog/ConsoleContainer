namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessOutputDataEventArgs
    {
        public ProcessOutputData Data { get; }

        public ProcessOutputDataEventArgs(ProcessOutputData data)
        {
            Data = data;
        }
    }
}
