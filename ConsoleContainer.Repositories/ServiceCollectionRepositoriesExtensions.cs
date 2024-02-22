using ConsoleContainer.Domain;
using ConsoleContainer.Repositories.Configuration;
using ConsoleContainer.Repositories.Files;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.Repositories
{
    public static class ServiceCollectionRepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, Action<IRepositoryOptions>? configure = null)
        {
            var options = new RepositoryOptions();
            configure?.Invoke(options);

            return services.AddRepositories(options);
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IRepositoryOptions options)
        {
            services.AddSingleton(options);
            services.AddSingleton<IFileRepository<ProcessGroupCollection>>(new FileRepository<ProcessGroupCollection>(options, "processGroups.json", () => new ProcessGroupCollection()));
            services.AddTransient<IProcessGroupCollectionRepository, ProcessGroupCollectionRepository>();
            return services;
        }
    }
}
