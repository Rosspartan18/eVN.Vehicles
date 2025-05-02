using eVN.Vehicles.Infrastructure.Models;
using LiteDB;

namespace eVN.Vehicles.Infrastructure.Storage
{
    public class VehicleDataStore : LiteDbQueryableDataStore<Vehicle>
    {
        public VehicleDataStore(ILiteDatabase database) : base(database)
        {
        }
    }
}
