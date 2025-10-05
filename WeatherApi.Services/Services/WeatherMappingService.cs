using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WeatherApi.Services.DTOs;
using WeatherApi.Services.Interfaces;
using WeatherApi.Services.Models;

namespace WeatherApi.Services
{
    public class WeatherMappingService : IWeatherMappingService
    {
        private readonly IMapper _mapper;

        public WeatherMappingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Maps NASA weather response to WeatherDataDto for frontend consumption.
        /// </summary>
        /// <param name="nasaResponse">Deserialized NASA weather response.</param>
        /// <returns>WeatherDataDto with temperature, condition, humidity, wind speed, and precipitation.</returns>
        public WeatherDataDto MapToWeatherData(NasaWeatherResponse nasaResponse)
        {
            var dateKey = DateTime.Now.ToString("yyyyMMdd"); // Or pass as parameter
            var values = nasaResponse.Properties?.Parameter?.Values;
            double temperature = values != null && values.ContainsKey("T2M") && values["T2M"].ContainsKey(dateKey)
                ? values["T2M"][dateKey] : -999;
            double precipitation = values != null && values.ContainsKey("PRECTOTCORR") && values["PRECTOTCORR"].ContainsKey(dateKey)
                ? values["PRECTOTCORR"][dateKey] : 0;
            double windSpeed = values != null && values.ContainsKey("WS2M") && values["WS2M"].ContainsKey(dateKey)
                ? values["WS2M"][dateKey] : -999;
            double humidity = values != null && values.ContainsKey("RH2M") && values["RH2M"].ContainsKey(dateKey)
                ? values["RH2M"][dateKey] : 0;

            // Derive condition from parameters
            string condition;
            if (precipitation > 0.5)
                condition = "Rainy";
            else if (temperature > 25)
                condition = "Hot";
            else if (temperature < 5)
                condition = "Cold";
            else
                condition = "Clear";

            var warning = CalculateWarning(temperature, precipitation, windSpeed);

            return new WeatherDataDto
            {
                Temperature = temperature,
                Condition = condition,
                Humidity = humidity,
                WindSpeed = windSpeed,
                Precipitation = precipitation,
                Warning = warning
            };
        }

        /// <summary>
        /// Calculates weather warning based on thresholds for wind, rain, temperature.
        /// </summary>
        /// <param name="temperature">Temperature in Celsius</param>
        /// <param name="precipitation">Precipitation in mm</param>
        /// <param name="windSpeed">Wind speed in m/s</param>
        /// <returns>WeatherWarning enum value or null</returns>
        public WeatherWarning? CalculateWarning(double temperature, double precipitation, double windSpeed)
        {
            if (windSpeed >= 15) // threshold for high wind (m/s)
                return WeatherWarning.HighWind;
            if (precipitation >= 10) // threshold for heavy rain (mm)
                return WeatherWarning.HeavyRain;
            if (temperature <= 0) // threshold for very cold (Celsius)
                return WeatherWarning.VeryCold;
            if (temperature >= 35) // threshold for very hot (Celsius)
                return WeatherWarning.VeryHot;
            return null;
        }

        public List<ForecastDayDto> MapToForecast(NasaWeatherResponse nasaResponse)
        {
            var forecast = new List<ForecastDayDto>();
            // NASA POWER API does not provide multi-day forecast in this endpoint
            // You can extend this logic if you use a different endpoint
            return forecast;
        }
    }
}