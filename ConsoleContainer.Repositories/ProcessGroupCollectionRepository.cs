using ConsoleContainer.Domain;
using ConsoleContainer.Repositories.Files;

namespace ConsoleContainer.Repositories
{
    public class ProcessGroupCollectionRepository : IProcessGroupCollectionRepository
    {
        private readonly IFileRepository<ProcessGroupCollection> fileRepository;

        public ProcessGroupCollectionRepository()
            : this(new FileRepository<ProcessGroupCollection>("processGroups.json", () => new ProcessGroupCollection()))
        {
        }

        public ProcessGroupCollectionRepository(IFileRepository<ProcessGroupCollection> fileRepository)
        {
            this.fileRepository = fileRepository;
        }

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
