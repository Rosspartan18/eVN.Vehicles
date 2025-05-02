using eVN.Vehicles.Services;
using Microsoft.AspNetCore.Mvc;

namespace eVN.Vehicles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    /// <summary>
    /// Controller for augmenting vehicle data with external sources.
    /// </summary>
    public class VehicleAugmentingController : ControllerBase
    {
        private readonly ILogger<VehicleAugmentingController> _logger;
        private readonly IVehicleAugmentingService _vehicleAugmentingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAugmentingController"/> class.
        /// </summary>
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