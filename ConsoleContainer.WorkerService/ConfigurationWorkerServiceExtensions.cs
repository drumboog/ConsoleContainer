namespace ConsoleContainer.WorkerService
{
    public static class ConfigurationWorkerServiceExtensions
    {
        public static T GetRequiredValue<T>(this IConfiguration config, string sectionName)
            where T : class
        {
            var section = config.GetSection(sectionName) ?? throw new Exception($"{sectionName} configuration section not found.");
            return section.Get<T>() ?? throw new Exception($"{sectionName} section could not be built.");
        }
    }
}
