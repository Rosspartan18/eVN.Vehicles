using LiteDB;

namespace env.Vehicles.Infrastructure.Models
{
    public class Vehicle
    {
        public int DealerId { get; set; }

        [BsonId]
        public string VIN { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Axles { get; set; }
        public string EngineModel { get; set; }
        public string FuelTankType { get; set; }
        public string VehicleType { get; set; }
        public string VehicleDescriptor { get; set; }

    }
}
