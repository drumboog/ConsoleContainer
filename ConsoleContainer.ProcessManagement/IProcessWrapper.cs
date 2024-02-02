namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessWrapper
    {
        event EventHandler<ProcessOutputDataEventArgs>? OutputDataReceived;
        event EventHandler<ProcessStateChangedEventArgs>? StateChanged;

        int? ProcessId { get; }

        string ProcessLocator { get; }
        ProcessDetails ProcessDetails { get; }

        ProcessState State { get; }

        IReadOnlyCollection<ProcessOutputData> OutputData { get; }

        void StartProcess();
        void StopProcess();
    }
}
