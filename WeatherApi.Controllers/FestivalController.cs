using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;
using WeatherApi.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// Provides festival data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FestivalController : ControllerBase
    {
        private readonly IFestivalService _festivalService;
        private readonly ILogger<FestivalController> _logger;

        public FestivalController(IFestivalService festivalService, ILogger<FestivalController> logger)
        {
            _festivalService = festivalService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all festivals.
        /// </summary>
        /// <returns>List of festivals.</returns>
        [HttpGet]
        public IActionResult GetFestivals()
        {
            try
            {
                var festivals = _festivalService.GetAllFestivals();
                return Ok(festivals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching festival data.");
                return StatusCode(500, "An error occurred while fetching festival data.");
            }
        }
    }
}
