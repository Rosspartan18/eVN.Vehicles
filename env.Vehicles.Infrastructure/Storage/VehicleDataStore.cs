using LiteDB;

namespace env.Vehicles.Infrastructure.Storage
{
    public class VehicleDataStore : LiteDbQueryableDataStore<Models.Vehicle>
    {
        public VehicleDataStore(ILiteDatabase database) : base(database)
        {
        }
    }
}
