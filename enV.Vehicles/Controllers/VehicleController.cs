using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using enV.Vehicles.Services;
using Microsoft.AspNetCore.Mvc;

namespace enV.Vehicles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }


        [HttpPost("import-csv")]
        public async Task<IActionResult> ImportFromCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }

            try
            {
                using var stream = file.OpenReadStream();

                var recordCount = await _vehicleService.ImportFromCsv(file.Name, stream).ConfigureAwait(false);

                return Ok(new { Message = "CSV imported successfully.", RecordCount = recordCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while importing CSV.");
                return StatusCode(500, "An error occurred while processing the file.");
            }
        }

        [HttpGet("get-by-vin/{vin}")]
        public async Task<IActionResult> GetVehicleByVin(string vin)
        {
            var vehicle = await _vehicleService.GetVehicleByVin(vin).ConfigureAwait(false);

            if (vehicle == null)
            {
                return NotFound(new { Message = $"Vehicle with VIN '{vin}' not found." });
            }

            return Ok(vehicle);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListVehicles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? dealerId = null, [FromQuery] DateTimeOffset? modifiedAfterDateTimeOffset = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            try
            {
                var (vehicles, totalCount) = await _vehicleService.GetVehiclesAsync(pageNumber, pageSize, dealerId, modifiedAfterDateTimeOffset).ConfigureAwait(false);

                return Ok(new
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Vehicles = vehicles
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while listing vehicles.");
                return StatusCode(500, "An error occurred while listing vehicles.");
            }
        }
    }
}
