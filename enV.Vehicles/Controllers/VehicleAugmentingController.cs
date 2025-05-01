using enV.Vehicles.Services;
using Microsoft.AspNetCore.Mvc;

namespace enV.Vehicles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleAugmentingController : ControllerBase
    {
        private readonly ILogger<VehicleAugmentingController> _logger;
        private readonly IVehicleAugmentingService _vehicleAugmentingService;

        public VehicleAugmentingController(IVehicleAugmentingService vehicleAugmentingService, ILogger<VehicleAugmentingController> logger)
        {
            _logger = logger;
            _vehicleAugmentingService = vehicleAugmentingService;
        }

        /// <summary>
        /// Augments vehicle data using external data from the NHTSA vPIC API.
        /// </summary>
        /// <returns>A response indicating the success or failure of the operation.</returns>
        [HttpPost("augment")]
        public async Task<IActionResult> AugmentVehicles()
        {
            try
            {
                var augmentedCount = await _vehicleAugmentingService.AugmentAllVehiclesAsync().ConfigureAwait(false);
                return Ok(new { Message = "Vehicles augmented successfully.", AugmentedCount = augmentedCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while augmenting vehicles.");
                return StatusCode(500, "An error occurred while augmenting vehicles.");
            }
        }
    }
}