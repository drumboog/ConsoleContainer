namespace ConsoleContainer.ProcessManagement
{
    public class ProcessWrapperFactory : IProcessWrapperFactory
    {
        public IProcessWrapper CreateProcessWrapper(ProcessDetails processDetails)
        {
            return new ProcessWrapper(processDetails);
        }
    }
}
