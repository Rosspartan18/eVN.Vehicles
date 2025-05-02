using eVN.Vehicles.Services.Entities;

namespace eVN.Vehicles.Services
{
    public interface IVehicleService
    {
        Task<Vehicle?> GetVehicleByVin(string vin);
        Task<(int, Guid)> ImportFromCsv(string fileName, Stream stream);
        Task<(IEnumerable<VehicleListEntry> Vehicles, int TotalCount)> GetVehiclesAsync(int pageNumber, int pageSize, int? dealerId = null, DateTimeOffset? modifiedAfterDateTimeOffset = null);
        Task<string?> RetrieveCsv(Guid id, Stream stream);
    }
}