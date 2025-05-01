using CsvHelper.Configuration.Attributes;

namespace enV.Vehicles.DataTransferObjects
{
    public record VehicleDto
    {
        [Name("dealerId")]
        public required int DealerId { get; set; }
        [Name("vin")]
        public required string VIN { get; set; }
        [Name("modifiedDate")]
        public required DateTimeOffset ModifiedDate { get; set; }
    }
}
