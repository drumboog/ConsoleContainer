using ConsoleContainer.Contracts;
using System.Net.Http.Json;

namespace ConsoleContainer.WorkerService.Client
{
    public class WorkerServiceClient(
        HttpClient httpClient
    ) : IWorkerServiceClient
    {
        public async Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync("processGroup");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProcessGroupSummaryDto>>(cancellationToken);
            return result!;
        }
    }
}
