namespace ConsoleContainer.Wpf.Repositories
{
    internal interface IRepository<T>
    {
        T Read();
        void Save(T data);
    }
}
