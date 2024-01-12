using ConsoleContainer.Wpf.Serialization;

namespace ConsoleContainer.Wpf.Repositories
{
    internal abstract class Repository<T> : IRepository<T>
    {
        private readonly FileManager<T> fileManager;

        protected Repository(string fileName, Func<T> factory)
            : this(fileName, new JsonBinarySerializer(true), factory)
        {
        }

        protected Repository(string fileName, ISerializer serializer, Func<T> factory)
        {
            fileManager = new FileManager<T>(fileName, serializer, factory);
        }

        public T Read()
        {
            return fileManager.Read();
        }

        public void Save(T data)
        {
            fileManager.Save(data);
        }
    }
}
