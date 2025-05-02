using System.Globalization;
using CsvHelper;
using env.Vehicles.Infrastructure.Storage;
using enV.Vehicles.DataTransferObjects;
using enV.Vehicles.Services.Entities;

namespace enV.Vehicles.Services
{
    /// <summary>
    /// Provides services for managing vehicle data.
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> _queryableDataStore;
        private readonly IBackingFileStore _backingFileStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        public VehicleService(IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle> queryableDataStore, IBackingFileStore backingFileStore)
        {
            _queryableDataStore = queryableDataStore;
            _backingFileStore = backingFileStore;
        }

        public async Task<(int, Guid)> ImportFromCsv(string fileName, Stream stream)
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
                ModifiedDate = record.ModifiedDate,
            });

            var upsertCount = await _queryableDataStore.UpsertManyAsync(mappedRecords).ConfigureAwait(false);

            return (upsertCount, fileStoreId);
        }

        public async Task<string?> RetrieveCsv(Guid id, Stream stream)
        {

            var fileName = await _backingFileStore.RetrieveFileAsync(id, stream);

            return fileName;
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
                Axles = vehicle.Axles,
                EngineModel = vehicle.EngineModel,
                FuelTankType = vehicle.FuelTankType,
                VehicleType = vehicle.VehicleType,
                VehicleDescriptor = vehicle.VehicleDescriptor,
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
