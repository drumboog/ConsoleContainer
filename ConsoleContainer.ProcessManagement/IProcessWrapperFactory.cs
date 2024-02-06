namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessWrapperFactory
    {
        IProcessWrapper CreateProcessWrapper(ProcessDetails processDetails);
    }
}
