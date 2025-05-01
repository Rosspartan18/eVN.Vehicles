using System.Net.Http;
using System.Text.Json;
using env.Vehicles.Infrastructure;

namespace enV.Vehicles.Services
{
    public class VehicleAugmentingService : IVehicleAugmentingService
    {
        private readonly IQueryableDataStore _queryableDataStore;
        private readonly HttpClient _httpClient;
        public VehicleAugmentingService(IQueryableDataStore queryableDataStore, IHttpClientFactory httpClientFactory)
        {
            _queryableDataStore = queryableDataStore;

            _httpClient = httpClientFactory.CreateClient("nhtsa");
        }

        public async Task<int> AugmentAllVehiclesAsync()
        {
            var vehicles = await _queryableDataStore.GetAllAsync<env.Vehicles.Infrastructure.Models.Vehicle>().ConfigureAwait(false);

            foreach (var vehicle in vehicles)
            {
                var response = await _httpClient.GetAsync($"/api/vehicles/DecodeVin/{vehicle.VIN}?format=json");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var vpicData = JsonSerializer.Deserialize<VpicResponse>(jsonResponse);

                    if (vpicData != null)
                    {
                        // Augment the vehicle with data from vPIC
                        vehicle.Make = vpicData.Results.FirstOrDefault(r => r.Variable == "Make")?.Value ?? vehicle.Make;
                        vehicle.Model = vpicData.Results.FirstOrDefault(r => r.Variable == "Model")?.Value ?? vehicle.Model;
                        vehicle.Year = vpicData.Results.FirstOrDefault(r => r.Variable == "Model Year")?.Value ?? vehicle.Year;

                        // Update the vehicle in the data store
                        await _queryableDataStore.UpdateAsync(vehicle).ConfigureAwait(false);
                    }
                }
            }

            return vehicles.Count();
        }
    }

    public class VpicResponse
    {
        public List<VpicResult> Results { get; set; }
    }

    public class VpicResult
    {
        public string Variable { get; set; }
        public string Value { get; set; }
    }
}
