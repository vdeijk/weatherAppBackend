using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;
using WeatherApi.Services.Interfaces;
using WeatherApi.Services.Models;
using Microsoft.Extensions.Logging;
using WeatherApi.Services.DTOs;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// Provides weather data for a given location and date.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly INasaWeatherService _nasaWeatherService;
        private readonly IWeatherMappingService _weatherMappingService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(INasaWeatherService nasaWeatherService, IWeatherMappingService weatherMappingService, ILogger<WeatherController> logger)
        {
            _nasaWeatherService = nasaWeatherService;
            _weatherMappingService = weatherMappingService;
            _logger = logger;
        }

        /// <summary>
        /// Gets weather data for the specified latitude, longitude, and date.
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees.</param>
        /// <param name="longitude">Longitude in decimal degrees.</param>
        /// <param name="date">Date in yyyy-MM-dd format (optional).</param>
        /// <returns>Weather data DTO or error response.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(WeatherDataDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WeatherDataDto>> GetWeather(double latitude = 34.05, double longitude = -118.25, string? date = null)
        {
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                _logger.LogWarning("Invalid latitude or longitude: {Latitude}, {Longitude}", latitude, longitude);
                return BadRequest("Invalid latitude or longitude.");
            }

            DateTime targetDate;
            if (string.IsNullOrEmpty(date))
            {
                targetDate = DateTime.UtcNow;
            }
            else if (!DateTime.TryParse(date, out targetDate))
            {
                _logger.LogWarning("Invalid date format: {Date}", date);
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");
            }

            try
            {
                var nasaRaw = await _nasaWeatherService.GetWeatherDataAsync(latitude, longitude, targetDate);
                _logger.LogInformation("NASA Raw Response: {NasaRaw}", nasaRaw);
                var nasaResponse = System.Text.Json.JsonSerializer.Deserialize<NasaWeatherResponse>(nasaRaw);
                if (nasaResponse == null)
                {
                    _logger.LogWarning("Malformed NASA data received.");
                    return BadRequest("Malformed NASA data received.");
                }
                var weatherDto = _weatherMappingService.MapToWeatherData(nasaResponse);
                if (weatherDto.Temperature == -999 && weatherDto.Precipitation == 0 && weatherDto.WindSpeed == -999)
                {
                    _logger.LogWarning("No weather data available for the specified date/location.");
                    return NotFound("No weather data available for the specified date/location.");
                }
                return Ok(weatherDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data.");
                return StatusCode(500, "An error occurred while fetching weather data.");
            }
        }
    }
}