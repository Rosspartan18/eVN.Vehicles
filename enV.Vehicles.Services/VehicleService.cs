using System.Globalization;
using CsvHelper;
using env.Vehicles.Infrastructure;
using enV.Vehicles.DataTransferObjects;
using enV.Vehicles.Services.Entities;

namespace enV.Vehicles.Services
{

    public class VehicleService : IVehicleService
    {
        private readonly IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> _queryableDataStore;
        private readonly IBackingFileStore _backingFileStore;
        public VehicleService(IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> queryableDataStore, IBackingFileStore backingFileStore)
        {
            _queryableDataStore = queryableDataStore;
            _backingFileStore = backingFileStore;
        }

        public async Task<int> ImportFromCsv(string fileName, Stream stream)
        {
            // Read the stream into the BackingFileStore
            var fileStoreId = Guid.NewGuid();
            await _backingFileStore.StoreFileAsync(fileStoreId, fileName, stream);

            // Read the csv and store the records in the queryable DataStore
            stream.Position = 0;
            using var memoryStreamReader = new StreamReader(stream);
            using var csvReader = new CsvReader(memoryStreamReader, CultureInfo.InvariantCulture);

            var records = csvReader.GetRecords<VehicleDto>().ToList();
                     
             var mappedRecords = records.Select(record => new env.Vehicles.Infrastructure.Models.Vehicle
             {
                VIN = record.VIN,
                DealerId = record.DealerId,
                ModifiedDate = record.ModifiedDate
            });

            return await _queryableDataStore.UpsertManyAsync(mappedRecords).ConfigureAwait(false);
        }

        public Task<Vehicle?> GetVehicleByVin(string vin)
        {
            var vehicle = _queryableDataStore.GetQueryable()
                .Where(v => v.VIN == vin)
                .FirstOrDefault();

            if (vehicle == null)
            {
                return Task.FromResult((Vehicle?)null);
            }

            var vehicleEntity = new Vehicle
            {
                DealerId = vehicle.DealerId,
                VIN = vehicle.VIN,
                ModifiedDate = vehicle.ModifiedDate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color,
                Mileage = vehicle.Mileage
            };

            return Task.FromResult ((Vehicle?)vehicleEntity);
        }

        public async Task<(IEnumerable<VehicleListEntry> Vehicles, int TotalCount)> GetVehiclesAsync(int pageNumber, int pageSize, int? dealerId = null, DateTimeOffset? modifiedAfterDateTimeOffset = null)
        {
            var query = _queryableDataStore.GetQueryable();

            if (dealerId != null)
            {
                query = query.Where(v => v.DealerId == dealerId);
            }

            if (modifiedAfterDateTimeOffset != null)
            {
                query = query.Where(v => v.ModifiedDate > modifiedAfterDateTimeOffset);
            }

            var totalCount = query.Count();

            var vehicles = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VehicleListEntry
                {
                    VIN = v.VIN,
                    DealerId = v.DealerId,
                    ModifiedDate = v.ModifiedDate
                });

            return (vehicles, totalCount);
        }
    }
}
