namespace enV.Vehicles.DataTransferObjects
{
    public record VehicleDto
    {
        public required int DealerId { get; set; }

        public required string VIN { get; set; }

        public required DateTimeOffset ModifiedDate { get; set; }
    }
}
