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
        public VehicleService(IQueryableDataStore queryableDataStore)
        {
            _queryableDataStore = queryableDataStore;
        }   

        public async Task<int> ImportFromCsv(StreamReader streamReader)
        {
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var records = csvReader.GetRecords<VehicleDto>().Select(record => new env.Vehicles.Infrastructure.Models.Vehicle
            {
                DealerId = record.DealerId,
                VIN = record.VIN,
                ModifiedDate = record.ModifiedDate
            });

            await _queryableDataStore.AddManyAsync(records).ConfigureAwait(false);

            return records.Count();

        }

        public async Task<Vehicle> GetVehicleByVin(string vin)
        {
            throw new NotImplementedException();
        }
    }
}
