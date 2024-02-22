using ConsoleContainer.Repositories.Configuration;
using ConsoleContainer.Repositories.Serialization;

namespace ConsoleContainer.Repositories.Files
{
    internal class FileRepository<T> : IFileRepository<T>
    {
        private readonly FileManager<T> fileManager;

        public FileRepository(IRepositoryOptions options, string fileName, Func<T> factory)
            : this(options, fileName, new JsonBinarySerializer(true), factory)
        {
        }

        public FileRepository(IRepositoryOptions options, string fileName, IBinarySerializer serializer, Func<T> factory)
        {
            fileManager = new FileManager<T>(options, fileName, serializer, factory);
        }

        public Task<T> ReadAsync()
        {
            return fileManager.ReadAsync();
        }

        public Task SaveAsync(T data)
        {
            return fileManager.SaveAsync(data);
        }
    }
}
