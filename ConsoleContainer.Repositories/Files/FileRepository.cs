using ConsoleContainer.Repositories.Serialization;

namespace ConsoleContainer.Repositories.Files
{
    internal class FileRepository<T> : IFileRepository<T>
    {
        private readonly FileManager<T> fileManager;

        public FileRepository(string fileName, Func<T> factory)
            : this(fileName, new JsonBinarySerializer(true), factory)
        {
        }

        public FileRepository(string fileName, IBinarySerializer serializer, Func<T> factory)
        {
            fileManager = new FileManager<T>(fileName, serializer, factory);
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
