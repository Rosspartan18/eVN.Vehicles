namespace enV.Vehicles.Services.Entities
{
    public class VehicleListEntry
    {
        public int DealerId { get; internal set; }
        public string VIN { get; internal set; }
        public DateTimeOffset ModifiedDate { get; internal set; }
    }
}
