using env.Vehicles.Infrastructure.Models;
using env.Vehicles.Infrastructure.Storage;
using Moq;

namespace enV.Vehicles.Services.Tests
{
    [TestClass]
    public class VehicleAugmentingServiceTests
    {
        private readonly Mock<IQueryableDataStore<Vehicle>> _dataStoreMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly VehicleAugmentingService _vehicleAugmentingService;

        public VehicleAugmentingServiceTests()
        {
            _dataStoreMock = new Mock<IQueryableDataStore<Vehicle>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _vehicleAugmentingService = new VehicleAugmentingService(_dataStoreMock.Object, _httpClientFactoryMock.Object);
        }

        [TestMethod]
        public async Task AugmentAllVehiclesAsync_ShouldReturnAugmentedCount()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle { VIN = "1", DealerId = 1, ModifiedDate = DateTimeOffset.UtcNow },
                new Vehicle { VIN = "2", DealerId = 1, ModifiedDate = DateTimeOffset.UtcNow }
            };

            _dataStoreMock.Setup(ds => ds.GetAllAsync()).ReturnsAsync(vehicles);

            // Act
            var augmentedCount = await _vehicleAugmentingService.AugmentAllVehiclesAsync();

            // Assert
            Assert.AreEqual(vehicles.Count, augmentedCount);
        }
    }
}
