
using enV.Vehicles.Services.Entities;

namespace enV.Vehicles.Services
{
    public interface IVehicleService
    {
        Task<Vehicle?> GetVehicleByVin(string vin);
        Task<int> ImportFromCsv(string fileName, Stream stream);
    }
}