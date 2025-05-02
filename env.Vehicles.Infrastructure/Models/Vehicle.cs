using LiteDB;

namespace eVN.Vehicles.Infrastructure.Models
{
    public class Vehicle
    {
        [BsonId]
        public required string VIN { get; set; }
        public required int DealerId { get; set; }
        public required DateTimeOffset ModifiedDate { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Axles { get; set; }
        public string? EngineModel { get; set; }
        public string? FuelTankType { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleDescriptor { get; set; }
    }
}
