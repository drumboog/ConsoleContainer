using Microsoft.Extensions.Logging;

namespace ConsoleContainer.ProcessManagement
{
    public class ProcessWrapperFactory<TKey>(
        ILogger<ProcessWrapper<TKey>> processWrapperLogger
    ) : IProcessWrapperFactory<TKey>
    {
        public IProcessWrapper<TKey> CreateProcessWrapper(TKey key, ProcessDetails processDetails)
        {
            return new ProcessWrapper<TKey>(processWrapperLogger, key, processDetails);
        }
    }
}
