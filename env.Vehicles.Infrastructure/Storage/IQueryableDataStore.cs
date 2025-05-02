namespace eVN.Vehicles.Infrastructure.Storage
{
    public interface IQueryableDataStore<T> where T : class
    {
        /// <summary>
        /// Adds an entity to the data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(T entity);
        Task<int> UpsertManyAsync(IEnumerable<T> entities);
        Task DeleteAsync(int id);
        Task<IList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetQueryable();
        Task SaveChangesAsync();
        Task<bool> UpdateAsync(T entity);
    }
}