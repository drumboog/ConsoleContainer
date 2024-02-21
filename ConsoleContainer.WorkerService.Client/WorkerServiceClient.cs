using ConsoleContainer.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Json;

namespace ConsoleContainer.WorkerService.Client
{
    public class WorkerServiceClient(
        ILogger<WorkerServiceClient> logger,
        HttpClient httpClient
    ) : IWorkerServiceClient
    {
        public async Task<IEnumerable<ProcessGroupSummaryDto>> GetProcessGroupsAsync(CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync("processGroup", cancellationToken);
            return await ValidateResponseAsync<List<ProcessGroupSummaryDto>>(response, cancellationToken);
        }

        public async Task CreateProcessGroupAsync(ProcessGroupDto processGroup)
        {
            var response = await httpClient.PostAsJsonAsync("processGroup", processGroup);
            await ValidateResponseAsync(response);
        }

        public async Task UpdateProcessGroupAsync(Guid processGroupId, ProcessGroupUpdateDto processGroup)
        {
            var response = await httpClient.PutAsJsonAsync($"processGroup/{processGroupId}", processGroup);
            await ValidateResponseAsync(response);
        }

        public async Task DeleteProcessGroupAsync(Guid processGroupId)
        {
            var response = await httpClient.DeleteAsync($"processGroup/{processGroupId}");
            await ValidateResponseAsync(response);
        }

        public async Task CreateProcessAsync(Guid processGroupId, ProcessInformationDto processInformation)
        {
            var response = await httpClient.PostAsJsonAsync($"processGroup/{processGroupId}/process", processInformation);
            await ValidateResponseAsync(response);
        }

        public async Task UpdateProcessAsync(Guid processGroupId, Guid processLocator, ProcessInformationUpdateDto processInformation)
        {
            var response = await httpClient.PutAsJsonAsync($"processGroup/{processGroupId}/process/{processLocator}", processInformation);
            await ValidateResponseAsync(response);
        }

        public async Task StartProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var response = await httpClient.PutAsync($"processGroup/{processGroupId}/process/{processLocator}/start", new StringContent(string.Empty));
            await ValidateResponseAsync(response);
        }

        public async Task StopProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var response = await httpClient.PutAsync($"processGroup/{processGroupId}/process/{processLocator}/stop", new StringContent(string.Empty));
            await ValidateResponseAsync(response);
        }

        public async Task DeleteProcessAsync(Guid processGroupId, Guid processLocator)
        {
            var response = await httpClient.DeleteAsync($"processGroup/{processGroupId}/process/{processLocator}");
            await ValidateResponseAsync(response);
        }

        private async Task<T> ValidateResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            await ValidateResponseAsync(response, cancellationToken);

            var content = string.Empty;
            try
            {
                content = await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not read response content");
                throw;
            }

            try
            {
                var result = JsonConvert.DeserializeObject<T>(content, new StringEnumConverter())!;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Could not parse content as {typeof(T)}");
                throw;
            }
        }

        private async Task ValidateResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var logData = new { response.StatusCode, ResponseContent = content };
            var logDataString = JsonConvert.SerializeObject(logData);
            logger.LogError($"Response returned unsuccessful status code: {logDataString}");

            response.EnsureSuccessStatusCode();
        }
    }
}
