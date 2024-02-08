using ConsoleContainer.Domain;

namespace ConsoleContainer.Repositories
{
    public interface IProcessGroupCollectionRepository
    {
        Task<ProcessGroupCollection> ReadAsync();
        Task SaveAsync(ProcessGroupCollection data);
    }
}
