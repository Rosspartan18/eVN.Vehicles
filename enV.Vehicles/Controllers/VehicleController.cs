using enV.Vehicles.Services;
using Microsoft.AspNetCore.Mvc;

namespace enV.Vehicles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    /// <summary>
    /// Controller for managing vehicle data.
    /// </summary>
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleController"/> class.
        /// </summary>
        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }


        /// <summary>
        /// Imports the CSV file
        /// </summary>
        /// <param name="file">The CSV file tp be imported.</param>
        /// <returns>Number of records added and identifier for the imported Csv</returns>
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

                var (recordCount, storageId) = await _vehicleService.ImportFromCsv(file.Name, stream).ConfigureAwait(false);

                return Ok(new { Message = "CSV imported successfully.", RecordCount = recordCount, CsvIdentifier  = storageId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while importing CSV.");
                return StatusCode(500, "An error occurred while processing the file.");
            }
        }

        /// <summary>
        /// Retrieves the imported CSV file by its identifier.
        /// </summary>
        /// <param name="csvIdentifier">The unique identifier of the imported CSV file.</param>
        /// <returns>The CSV file as a downloadable response.</returns>
        [HttpGet("get-imported-csv/{csvIdentifier}")]
        public async Task<IActionResult> GetImportedCsv(Guid csvIdentifier)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var fileName = await _vehicleService.RetrieveCsv(csvIdentifier, memoryStream).ConfigureAwait(false);

                if (fileName == null)
                {
                    return NotFound(new { Message = $"CSV file with identifier '{csvIdentifier}' not found." });
                }

                memoryStream.Position = 0; // Reset the stream position for reading
                return File(memoryStream, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the imported CSV.");
                return StatusCode(500, "An error occurred while retrieving the imported CSV.");
            }
        }

        [HttpGet("get-by-vin/{vin}")]
        public async Task<IActionResult> GetVehicleByVin(string vin)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByVin(vin).ConfigureAwait(false);

                if (vehicle == null)
                {
                    return NotFound(new { Message = $"Vehicle with VIN '{vin}' not found." });
                }

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting vehicle by VIN.");
                return StatusCode(500, "An error occurred while getting vehicle by VIN.");
            }
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
