
namespace env.Vehicles.Infrastructure
{
    public interface IQueryableDataStore
    {
        /// <summary>
        /// Adds an entity to the data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync<T>(T entity) where T : class;
        Task AddManyAsync<T>(IEnumerable<T> entities) where T : class;
        Task DeleteAsync<T>(int id) where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<T> GetByIdAsync<T>(int id) where T : class;
        IQueryable<T> GetQueryable<T>() where T : class;
        Task SaveChangesAsync();
        Task UpdateAsync<T>(T entity) where T : class;
    }
}