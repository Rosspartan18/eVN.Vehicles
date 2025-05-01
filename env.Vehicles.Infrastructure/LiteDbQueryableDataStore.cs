using LiteDB;

namespace env.Vehicles.Infrastructure
{
    public class LiteDbQueryableDataStore : IQueryableDataStore
    {
        private readonly ILiteDatabase _database;

        public LiteDbQueryableDataStore(ILiteDatabase database)
        {
            _database = database;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return _database.GetCollection<T>().FindAll().AsQueryable();
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            var collection = _database.GetCollection<T>();
            return await Task.FromResult(collection.FindById(id));
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            var collection = _database.GetCollection<T>();
            return await Task.FromResult(collection.FindAll());
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            var collection = _database.GetCollection<T>();
            collection.Insert(entity);
            await Task.CompletedTask;
        }

        public async Task AddManyAsync<T>(IEnumerable<T> entities) where T : class
        {
            var collection = _database.GetCollection<T>();
            collection.InsertBulk(entities);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            var collection = _database.GetCollection<T>();
            collection.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync<T>(int id) where T : class
        {
            var collection = _database.GetCollection<T>();
            collection.Delete(id);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            // LiteDB automatically saves changes, so this is a no-op.
            await Task.CompletedTask;
        }
    }
}
