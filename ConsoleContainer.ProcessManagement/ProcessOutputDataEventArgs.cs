namespace ConsoleContainer.ProcessManagement
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
