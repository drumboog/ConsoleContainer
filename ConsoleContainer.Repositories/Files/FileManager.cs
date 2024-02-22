using ConsoleContainer.Repositories.Configuration;
using ConsoleContainer.Repositories.Serialization;

namespace ConsoleContainer.Repositories.Files
{
    internal class FileManager<T>(
        RepositoryOptions options,
        string fileName,
        IBinarySerializer serializer,
        Func<T> factory
    )
    {
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
            var path = Path.Combine(applicationDirectory, options.RootDirectoryName, fileName);
            return path;
        }
    }
}
