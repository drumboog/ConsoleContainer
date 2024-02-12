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

        public async Task CreateProcessGroupAsync(ProcessGroupDto processGroup)
        {
            var response = await httpClient.PostAsJsonAsync("processGroup", processGroup);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProcessGroupAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup)
        {
            var response = await httpClient.PutAsJsonAsync($"processGroup/{processGroupId}", processGroup);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProcessGroupAsync(Guid processGroupId)
        {
            var response = await httpClient.DeleteAsync($"processGroup/{processGroupId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateProcessAsync(Guid processGroupId, ProcessInformationDto processInformation)
        {
            var response = await httpClient.PostAsJsonAsync($"processGroup/{processGroupId}/process", processInformation);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProcessAsync(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto processInformation)
        {
            var response = await httpClient.PutAsJsonAsync($"processGroup/{processGroupId}/process/{processLocator}", processInformation);
            response.EnsureSuccessStatusCode();
        }

        public async Task StartProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var response = await httpClient.PutAsync($"processGroup/{processGroupId}/process/{processLocator}/start", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task StopProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var response = await httpClient.PutAsync($"processGroup/{processGroupId}/process/{processLocator}/stop", new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var response = await httpClient.DeleteAsync($"processGroup/{processGroupId}/process/{processLocator}");
            response.EnsureSuccessStatusCode();
        }
    }
}
