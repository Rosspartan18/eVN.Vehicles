using System.Globalization;
using CsvHelper;
using env.Vehicles.Infrastructure;
using enV.Vehicles.DataTransferObjects;
using enV.Vehicles.Services.Entities;

namespace enV.Vehicles.Services
{

    public class VehicleService : IVehicleService
    {
        private readonly IQueryableDataStore _queryableDataStore;
        private readonly IBackingFileStore _backingFileStore;
        public VehicleService(IQueryableDataStore queryableDataStore, IBackingFileStore backingFileStore)
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

            var records = csvReader.GetRecords<VehicleDto>().Select(record => new env.Vehicles.Infrastructure.Models.Vehicle
            {
                DealerId = record.DealerId,
                VIN = record.VIN,
                ModifiedDate = record.ModifiedDate
            });

            await _queryableDataStore.AddManyAsync(records).ConfigureAwait(false);

            return records.Count();
        }

        public Task<Vehicle?> GetVehicleByVin(string vin)
        {
            var vehicle = _queryableDataStore.GetQueryable<env.Vehicles.Infrastructure.Models.Vehicle>()
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
    }
}
