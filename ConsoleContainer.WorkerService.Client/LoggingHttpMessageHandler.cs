using Microsoft.Extensions.Logging;

namespace ConsoleContainer.WorkerService.Client
{
    public class LoggingHttpMessageHandler(
        ILogger<LoggingHttpMessageHandler> logger
    ) : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Guid httpRequestId = Guid.NewGuid();
            logger.LogInformation($"{httpRequestId}: Sending {request.Method}: {request.RequestUri}.");
            var response = await base.SendAsync(request, cancellationToken);
            logger.LogInformation($"{httpRequestId}: Response received with status {response.StatusCode} from {request.Method}: {request.RequestUri}.");
            return response;
        }
    }
}
