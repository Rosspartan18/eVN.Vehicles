using LiteDB;

namespace env.Vehicles.Infrastructure
{    
    /// <summary>
    /// Implementation of <see cref="IBackingFileStore"/> using LiteDB's FileStorage.
    /// </summary>
    public class LiteDbBackingFileStore : IBackingFileStore
    {
        private readonly ILiteDatabase _database;

        public LiteDbBackingFileStore(ILiteDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Stores a file in LiteDB's FileStorage.
        /// </summary>
        /// <param name="id">The unique identifier for the file.</param>
        /// <param name="fileName">The name of the file to store.</param>
        /// <param name="fileStream">The stream containing the file data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// The file is stored in LiteDB's FileStorage with the specified <paramref name="id"/> and <paramref name="fileName"/>.
        /// </remarks>
        public async Task StoreFileAsync(Guid id, string fileName, Stream fileStream)
        {
            var fileStorage = _database.GetStorage<Guid>();

            // Store the file in LiteDB's FileStorage
            fileStorage.Upload(id, fileName, fileStream);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves a file from LiteDB's FileStorage.
        /// </summary>
        /// <param name="id">The unique identifier of the file to retrieve.</param>
        /// <param name="stream">The stream to which the file data will be written.</param>
        /// <returns>
        /// A task that returns the file name if the file exists, or <c>null</c> if the file does not exist.
        /// </returns>
        /// <remarks>
        /// If the file exists, its data is written to the provided <paramref name="stream"/>, and the file name is returned.
        /// If the file does not exist, <c>null</c> is returned.
        /// </remarks>
        public async Task<string?> RetrieveFileAsync(Guid id, Stream stream)
        {
            var fileStorage = _database.GetStorage<Guid>();

            if (!fileStorage.Exists(id))
            {
                return null;
            }

            // Open the file from LiteDB's FileStorage
            var fileInfo = fileStorage.Download(id, stream);

            return await Task.FromResult(fileInfo.Filename);
        }
    }
}
