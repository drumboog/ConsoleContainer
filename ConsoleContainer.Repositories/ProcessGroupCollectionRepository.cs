using ConsoleContainer.Domain;
using ConsoleContainer.Repositories.Files;

namespace ConsoleContainer.Repositories
{
    public class ProcessGroupCollectionRepository (
        IFileRepository<ProcessGroupCollection> fileRepository
    ) : IProcessGroupCollectionRepository
    {
        public Task<ProcessGroupCollection> ReadAsync()
        {
            return fileRepository.ReadAsync();
        }

        public Task SaveAsync(ProcessGroupCollection data)
        {
            return fileRepository.SaveAsync(data);
        }
    }
}
