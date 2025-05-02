using LiteDB;

namespace eVN.Vehicles.Infrastructure.Storage
{
    /// <summary>
    /// Implementation of <see cref="IQueryableDataStore{T}"/> using LiteDB.
    /// </summary>
    public abstract class LiteDbQueryableDataStore<T> : IQueryableDataStore<T>, IDisposable where T : class
    {
        private readonly ILiteDatabase _database;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteDbQueryableDataStore{T}"/> class.
        /// </summary>
        public LiteDbQueryableDataStore(ILiteDatabase database)
        {
            _database = database;
        }

        public IQueryable<T> GetQueryable()
        {
            return _database.GetCollection<T>().FindAll().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var collection = _database.GetCollection<T>();
            return await Task.FromResult(collection.FindById(id));
        }

        public async Task<IList<T>> GetAllAsync()
        {
            var collection = _database.GetCollection<T>();
            return await Task.FromResult(collection.FindAll().ToList());
        }

        public async Task AddAsync(T entity)
        {
            var collection = _database.GetCollection<T>();
            collection.Insert(entity);
            await Task.CompletedTask;
        }

        public Task<int> UpsertManyAsync(IEnumerable<T> entities)
        {
            var collection = _database.GetCollection<T>();
            var addedAmount = collection.Upsert(entities);
            return Task.FromResult(addedAmount);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            var collection = _database.GetCollection<T>();
            var result = collection.Update(entity);
            return Task.FromResult(result);
        }

        public async Task DeleteAsync(int id)
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

        /// <summary>
        /// Disposes of the LiteDbQueryableDataStore and releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is called from Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose the LiteDB database instance
                    _database?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
