namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessOutputDataEventArgs
    {
        public ProcessDetails ProcessDetails { get; }
        public ProcessOutputData Data { get; }

        public ProcessOutputDataEventArgs(ProcessDetails processDetails, ProcessOutputData data)
        {
            ProcessDetails = processDetails;
            Data = data;
        }
    }
}
