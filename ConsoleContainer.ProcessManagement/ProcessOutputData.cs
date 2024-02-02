namespace ConsoleContainer.ProcessManagement
{
    public class ProcessOutputData
    {
        public string? Data { get; }
        public bool IsProcessError { get; }

        public ProcessOutputData(string? data, bool isProcessError = false)
        {
            Data = data;
            IsProcessError = isProcessError;
        }
    }
}
