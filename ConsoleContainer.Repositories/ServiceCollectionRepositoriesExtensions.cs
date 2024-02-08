using ConsoleContainer.Domain;
using ConsoleContainer.Repositories.Files;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleContainer.Repositories
{
    public static class ServiceCollectionRepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IFileRepository<ProcessGroupCollection>>(new FileRepository<ProcessGroupCollection>("processGroups.json", () => new ProcessGroupCollection()));
            services.AddTransient<IProcessGroupCollectionRepository, ProcessGroupCollectionRepository>();
            return services;
        }
    }
}
