namespace eVN.Vehicles.Services.Entities
{
    public class VehicleListEntry
    {
        public required string VIN { get; set; }
        public required int DealerId { get; set; }
        public required DateTimeOffset ModifiedDate { get; set; }
    }
}
