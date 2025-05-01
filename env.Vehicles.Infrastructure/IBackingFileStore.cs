
namespace env.Vehicles.Infrastructure
{
    /// <summary>
    /// Defines methods for storing and retrieving files in a backing file store.
    /// </summary>
    public interface IBackingFileStore
    {
        /// <summary>
        /// Stores a file in the backing file store.
        /// </summary>
        /// <param name="id">The unique identifier for the file.</param>
        /// <param name="fileName">The name of the file to store.</param>
        /// <param name="fileStream">The stream containing the file data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StoreFileAsync(Guid id, string fileName, Stream fileStream);

        /// <summary>
        /// Retrieves a file from the backing file store.
        /// </summary>
        /// <param name="id">The unique identifier of the file to retrieve.</param>
        /// <param name="stream">The stream to which the file data will be written.</param>
        /// <returns>
        /// A task that returns the file name if the file exists, or <c>null</c> if the file does not exist.
        /// </returns>
        Task<string?> RetrieveFileAsync(Guid id, Stream stream);
    }
}