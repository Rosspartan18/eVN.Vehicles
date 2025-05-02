namespace enV.Vehicles.Services.Entities
{    
    /// <summary>
    /// Represents a vehicle entity.
    /// </summary>
    public class Vehicle
    {
        public required string VIN { get; set; }
        public required int DealerId { get; set; }
        public required DateTimeOffset ModifiedDate { get; set; }
        public string? Make { get; internal set; }
        public string? Model { get; internal set; }
        public string? Year { get; internal set; }
        public string? Axles { get; set; }
        public string? EngineModel { get; set; }
        public string? FuelTankType { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleDescriptor { get; set; }
    }
}
