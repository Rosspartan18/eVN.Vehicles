using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using enV.Vehicles.Services.ServiceClient.Models;
using Microsoft.Extensions.Logging;

namespace enV.Vehicles.Services.ServiceClient
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
