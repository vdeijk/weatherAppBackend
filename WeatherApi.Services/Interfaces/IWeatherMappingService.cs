using System.Collections.Generic;
using WeatherApi.Services.DTOs;
using WeatherApi.Services.Models;

namespace WeatherApi.Services.Interfaces
{
    public interface IWeatherMappingService
    {
        WeatherDataDto MapToWeatherData(NasaWeatherResponse nasaResponse);
        List<ForecastDayDto> MapToForecast(NasaWeatherResponse nasaResponse);
    }
}
