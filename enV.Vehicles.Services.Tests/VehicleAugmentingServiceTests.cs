using eVN.Vehicles.Infrastructure.Models;
using eVN.Vehicles.Infrastructure.Storage;
using eVN.Vehicles.Services.ServiceClient;
using eVN.Vehicles.Services.ServiceClient.Models;
using Moq;

namespace eVN.Vehicles.Services.Tests
{
    [TestClass]
    public class VehicleAugmentingServiceTests
    {
        private readonly Mock<IQueryableDataStore<Vehicle>> _dataStoreMock;
        private readonly Mock<INhtsaServiceClient> _nhtsaServiceClient;
        private readonly VehicleAugmentingService _vehicleAugmentingService;

        public VehicleAugmentingServiceTests()
        {
            _dataStoreMock = new Mock<IQueryableDataStore<Vehicle>>();
            _nhtsaServiceClient = new Mock<INhtsaServiceClient>();
            _vehicleAugmentingService = new VehicleAugmentingService(_dataStoreMock.Object, _nhtsaServiceClient.Object);
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


            _dataStoreMock.Setup(ds => ds.UpdateAsync(It.IsAny<Vehicle>())).ReturnsAsync(true);
            _dataStoreMock.Setup(ds => ds.GetAllAsync()).ReturnsAsync(vehicles);
            _nhtsaServiceClient.Setup(n => n.DecodeVin(It.IsAny<string>()))
                .ReturnsAsync(new VpicResponse
                {
                    Results = new List<VpicResult>
                    {
                        new VpicResult
                        { Variable = "Make", Value = "TestMake" },
                    }
                });

            // Act
            var augmentedCount = await _vehicleAugmentingService.AugmentAllVehiclesAsync();

            // Assert
            Assert.AreEqual(vehicles.Count, augmentedCount);
        }
    }
}
