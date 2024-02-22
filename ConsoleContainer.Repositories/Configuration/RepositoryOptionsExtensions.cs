namespace ConsoleContainer.Repositories.Configuration
{
    public static class RepositoryOptionsExtensions
    {
        public static IRepositoryOptions WithRootDirectory(this IRepositoryOptions options, string directory)
        {
            options.RootDirectory = directory;
            return options;
        }
    }
}
