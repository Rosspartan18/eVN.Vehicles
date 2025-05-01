namespace enV.Vehicles.DataTransferObjects
{
    public record VehicleDto
    {
        public required int Id { get; set; }

        public required string VehicleIdentificationNumber { get; set; }

        public required DateTimeOffset ModifiedDate { get; set; }
    }
}
