using System.Net.Http.Json;
using eVN.Vehicles.Services.ServiceClient.Models;
using Microsoft.Extensions.Logging;

namespace eVN.Vehicles.Services.ServiceClient
{

    public sealed class NhtsaServiceClient : IDisposable, INhtsaServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NhtsaServiceClient> _logger;

        public NhtsaServiceClient(HttpClient httpClient, ILogger<NhtsaServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<VpicResponse?> DecodeVin(string vin)
        {
            var url = $"/api/vehicles/DecodeVin/{vin}?format=json";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var vpicData = await response.Content.ReadFromJsonAsync<VpicResponse>();

                return vpicData;
            }
            else
            {
                _logger.LogError($"Failed to decode VIN {vin}. Status code: {response.StatusCode}");
            }

            return null;
        }
        public void Dispose() => _httpClient?.Dispose();
    }
}
