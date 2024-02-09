namespace ConsoleContainer.ProcessManagement.Events
{
    public class ProcessOutputDataEventArgs<TKey>
    {
        public TKey ProcessKey { get; }
        public ProcessDetails ProcessDetails { get; }
        public ProcessOutputData Data { get; }

        public ProcessOutputDataEventArgs(TKey processKey, ProcessDetails processDetails, ProcessOutputData data)
        {
            ProcessKey = processKey;
            ProcessDetails = processDetails;
            Data = data;
        }
    }
}
