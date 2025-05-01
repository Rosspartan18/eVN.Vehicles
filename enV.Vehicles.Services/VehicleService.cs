using System.Globalization;
using CsvHelper;
using enV.Vehicles.DataTransferObjects;
using enV.Vehicles.Services.Entities;

namespace enV.Vehicles.Services
{
    public class VehicleService : IVehicleService
    {
        public async Task<int> ImportFromCsv(StreamReader streamReader)
        {
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var records = csvReader.GetRecords<VehicleDto>().ToList();

            return records.Count();
        }

        public async Task<Vehicle> GetVehicleByVin(string vin)
        {
            throw new NotImplementedException();
        }
    }
}
