using env.Vehicles.Infrastructure.Models;
using env.Vehicles.Infrastructure.Storage;
using enV.Vehicles.Services.ServiceClient;
using enV.Vehicles.Services.ServiceClient.Models;

namespace enV.Vehicles.Services
{
    /// <summary>
    /// Provides services for augmenting vehicle data with external sources.
    /// </summary>
    public class VehicleAugmentingService : IVehicleAugmentingService
    {
        private readonly IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> _queryableDataStore;
        private readonly INhtsaServiceClient _nhtsaServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAugmentingService"/> class.
        /// </summary>
        public VehicleAugmentingService(IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> queryableDataStore, INhtsaServiceClient nhtsaServiceClient)
        {
            _queryableDataStore = queryableDataStore;
            _nhtsaServiceClient = nhtsaServiceClient;
        }

        public async Task<int> AugmentAllVehiclesAsync()
        {
            var vehicles = (await _queryableDataStore.GetAllAsync().ConfigureAwait(false));

            // Foreach of the vehicles, augment them.
            var augmentTasks = vehicles.Select(v => AugmentVehicle(v));

            // When all the tasks are complete, summarize the results
            bool[] results = await Task.WhenAll(augmentTasks);

            // Return count of updated entries.
            return results.Count(b => b);
        }

        private async Task<bool> AugmentVehicle(Vehicle vehicle)
        {
            var vpicData = await _nhtsaServiceClient.DecodeVin(vehicle.VIN);
            if (vpicData == null)
            {
                return false;
            }

            // Augment the vehicle with data from vPIC
            bool needsUpdate = false;
            if (TryGetVariable(vpicData.Results, "Make", out var make))
            {
                vehicle.Make = make;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Model", out var model))
            {
                vehicle.Model = make;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Model Year", out var modelYear))
            {
                vehicle.Year = modelYear;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Axles", out var axles))
            {
                vehicle.Axles = axles;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Engine Model", out var engineModel))
            {

                vehicle.EngineModel = engineModel;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Fuel-Tank Type", out var fuelTankType))
            {
                vehicle.FuelTankType = fuelTankType;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Vehicle Type", out var vehicleType))
            {
                vehicle.VehicleType = vehicleType;
                needsUpdate = true;
            }

            if (TryGetVariable(vpicData.Results, "Vehicle Descriptor", out var vehicleDescriptor))
            {
                vehicle.VehicleDescriptor = vehicleDescriptor;
                needsUpdate = true;
            }

            // Update the vehicle in the data store
            if (needsUpdate)
            {
                return await _queryableDataStore.UpdateAsync(vehicle).ConfigureAwait(false);
            }
            else
            {
                return false;
            }
        }

        private static bool TryGetVariable(List<VpicResult> vpicResults, string name, out string value)
        {
            var vpicResult = vpicResults.FirstOrDefault(x => x.Variable == name);

            if (vpicResult == null || vpicResult.Value == null)
            {
                value = string.Empty;
                return false;
            }
            else
            {
                value = vpicResult.Value;
                return true;
            }
        }
    }
}
