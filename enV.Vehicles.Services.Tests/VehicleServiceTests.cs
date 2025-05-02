using env.Vehicles.Infrastructure.Models;
using env.Vehicles.Infrastructure.Storage;
using Moq;

namespace enV.Vehicles.Services.Tests
{
    [TestClass]
    public class VehicleServiceTests
    {
        private readonly Mock<IQueryableDataStore<Vehicle>> _dataStoreMock;

        private readonly Mock<IBackingFileStore> _backingFileStoreMock;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _dataStoreMock = new Mock<IQueryableDataStore<Vehicle>>();
            _backingFileStoreMock = new Mock<IBackingFileStore>();
            _vehicleService = new VehicleService(_dataStoreMock.Object, _backingFileStoreMock.Object);
        }

        [TestMethod]
        public async Task GetVehicleByVin_ShouldReturnVehicle_WhenVehicleExists()
        {
            // Arrange
            var vin = "1HGCM82633A123456";
            var expectedVehicle = new Vehicle { VIN = vin, DealerId = 1, ModifiedDate = DateTimeOffset.UtcNow };
            var storedVehicles = new List<Vehicle> 
            {
                new Vehicle { VIN = vin, DealerId = 1, ModifiedDate = DateTimeOffset.UtcNow }
            };
            _dataStoreMock.Setup(ds => ds.GetQueryable()).Returns(storedVehicles.AsQueryable());

            // Act
            var result = await _vehicleService.GetVehicleByVin(vin);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(vin, result.VIN);
        }

        [TestMethod]
        public async Task GetVehicleByVin_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vin = "1HGCM82633A123456";
            var storedVehicles = new List<Vehicle>
            {
            };
            _dataStoreMock.Setup(ds => ds.GetQueryable()).Returns(storedVehicles.AsQueryable());

            // Act
            var result = await _vehicleService.GetVehicleByVin(vin);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ImportFromCsv_ShouldReturnRecordCountAndStorageId()
        {
            // Arrange
            var fileName = "vehicles.csv";
            var stream = new MemoryStream();
            var expectedRecordCount = 10;
            var expectedStorageId = Guid.NewGuid();

            _dataStoreMock.Setup(ds => ds.UpsertManyAsync(It.IsAny<IEnumerable<Vehicle>>()))
                .ReturnsAsync(expectedRecordCount);

            // Act
            var (recordCount, storageId) = await _vehicleService.ImportFromCsv(fileName, stream);

            // Assert
            Assert.AreEqual(expectedRecordCount, recordCount);
            Assert.AreNotEqual(Guid.Empty, storageId);
            _dataStoreMock.Verify(ds => ds.UpsertManyAsync(It.IsAny<IEnumerable<Vehicle>>()), Times.Once);
        }

        [TestMethod]
        public async Task GetVehiclesAsync_ShouldReturnPaginatedVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle { VIN = "1", DealerId = 1, ModifiedDate = DateTimeOffset.UtcNow },
                new Vehicle { VIN = "2", DealerId = 1, ModifiedDate = DateTimeOffset.UtcNow }
            };

            _dataStoreMock.Setup(ds => ds.GetQueryable()).Returns(vehicles.AsQueryable());

            // Act
            var (result, totalCount) = await _vehicleService.GetVehiclesAsync(1, 10);

            // Assert
            Assert.AreEqual(vehicles.Count, totalCount);
            Assert.AreEqual(vehicles.Count, result.Count());
        }
    }
}
