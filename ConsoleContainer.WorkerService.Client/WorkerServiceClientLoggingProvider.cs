using Microsoft.Extensions.Logging;

namespace ConsoleContainer.WorkerService.Client
{
    public class WorkerServiceClientLoggingProvider(
        ILogger logger
    ) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return logger;
        }

        public void Dispose()
        {
        }
    }
}
