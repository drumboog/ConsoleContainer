namespace ConsoleContainer.ProcessManagement
{
    public interface IProcessWrapperFactory<TKey>
    {
        IProcessWrapper<TKey> CreateProcessWrapper(TKey key, ProcessDetails processDetails);
    }
}
