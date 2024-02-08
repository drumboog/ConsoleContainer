using ConsoleContainer.Repositories.Serialization;

namespace ConsoleContainer.Repositories.Files
{
    internal class FileManager<T>
    {
        private readonly string fileName;
        private readonly IBinarySerializer serializer;
        private readonly Func<T> factory;

        public FileManager(string fileName, IBinarySerializer serializer, Func<T> factory)
        {
            this.fileName = fileName;
            this.serializer = serializer;
            this.factory = factory;
        }

        public async Task<T> ReadAsync()
        {
            try
            {
                var data = await File.ReadAllBytesAsync(GetFilePath());
                return serializer.Deserialize<T>(data) ?? factory();
            }
            catch
            {
                return factory();
            }
        }

        public async Task SaveAsync(T obj)
        {
            var data = serializer.Serialize(obj);
            var filePath = GetFilePath();
            var dir = Path.GetDirectoryName(filePath);
            if (dir is not null)
            {
                Directory.CreateDirectory(dir);
            }
            await File.WriteAllBytesAsync(filePath, data);
        }

        private string GetFilePath()
        {
            var applicationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(applicationDirectory, "ConsoleContainer", fileName);
            return path;
        }
    }
}
