using ConsoleContainer.Wpf.Domain;

namespace ConsoleContainer.Wpf.Repositories
{
    internal class ProcessGroupCollectionRepository : Repository<ProcessGroupCollection>
    {
        public ProcessGroupCollectionRepository()
            : base("processGroups.json", () => new ProcessGroupCollection())
        {
        }
    }
}
