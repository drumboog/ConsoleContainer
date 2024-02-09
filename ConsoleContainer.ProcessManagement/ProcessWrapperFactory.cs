namespace ConsoleContainer.ProcessManagement
{
    public class ProcessWrapperFactory<TKey> : IProcessWrapperFactory<TKey>
    {
        public IProcessWrapper<TKey> CreateProcessWrapper(TKey key, ProcessDetails processDetails)
        {
            return new ProcessWrapper<TKey>(key, processDetails);
        }
    }
}
