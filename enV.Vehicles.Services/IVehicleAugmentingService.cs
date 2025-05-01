namespace enV.Vehicles.Services
{
    public interface IVehicleAugmentingService
    {
        Task<int> AugmentAllVehiclesAsync();
    }
}