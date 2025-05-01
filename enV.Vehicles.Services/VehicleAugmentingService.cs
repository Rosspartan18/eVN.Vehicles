using System.Net.Http.Json;
using System.Text.Json;
using env.Vehicles.Infrastructure;

namespace enV.Vehicles.Services
{
    public class VehicleAugmentingService : IVehicleAugmentingService
    {
        private readonly IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> _queryableDataStore;
        private readonly HttpClient _httpClient;
        public VehicleAugmentingService(IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> queryableDataStore, IHttpClientFactory httpClientFactory)
        {
            _queryableDataStore = queryableDataStore;

            _httpClient = httpClientFactory.CreateClient("nhtsa");
        }

        public async Task<int> AugmentAllVehiclesAsync()
        {
            var vehicles = (await _queryableDataStore.GetAllAsync().ConfigureAwait(false));

            int updatedCount = 0;
            foreach (var vehicle in vehicles)
            {
                var response = await _httpClient.GetAsync($"/api/vehicles/DecodeVin/{vehicle.VIN}?format=json");
                if (response.IsSuccessStatusCode)
                {
                    var vpicData = await response.Content.ReadFromJsonAsync<VpicResponse>();

                    if (vpicData != null)
                    {
                        // Augment the vehicle with data from vPIC
                        vehicle.Make = vpicData.Results.FirstOrDefault(r => r.Variable == "Make")?.Value ?? vehicle.Make;
                        vehicle.Model = vpicData.Results.FirstOrDefault(r => r.Variable == "Model")?.Value ?? vehicle.Model;
                        vehicle.Year = vpicData.Results.FirstOrDefault(r => r.Variable == "Model Year")?.Value ?? vehicle.Year;

                        // Update the vehicle in the data store
                        var result = await _queryableDataStore.UpdateAsync(vehicle).ConfigureAwait(false);

                        if (result)
                        {
                            updatedCount++;
                        }
                    }
                }
            }

            return updatedCount;
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
