
using enV.Vehicles.Services.ServiceClient.Models;

namespace enV.Vehicles.Services.ServiceClient
{
    public interface INhtsaServiceClient
    {
        Task<VpicResponse?> DecodeVin(string vin);
    }
}