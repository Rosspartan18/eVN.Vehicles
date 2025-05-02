namespace eVN.Vehicles.Services
{
    /// <summary>
    /// Defines methods for augmenting vehicle data with external sources.
    /// </summary>
    public interface IVehicleAugmentingService
    {
        /// <summary>
        /// Augments all vehicles in the data store with external data.
        /// </summary>
        /// <returns>The number of vehicles successfully augmented.</returns>
        Task<int> AugmentAllVehiclesAsync();
    }
}
