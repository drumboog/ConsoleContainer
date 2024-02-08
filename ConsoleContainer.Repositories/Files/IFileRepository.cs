namespace ConsoleContainer.Repositories.Files
{
    public interface IFileRepository<T>
    {
        Task<T> ReadAsync();
        Task SaveAsync(T data);
    }
}
