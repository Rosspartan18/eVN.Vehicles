using eVN.Vehicles.Services.ServiceClient.Models;

namespace eVN.Vehicles.Services.ServiceClient
{
    public interface INhtsaServiceClient
    {
        Task<VpicResponse?> DecodeVin(string vin);
    }
}