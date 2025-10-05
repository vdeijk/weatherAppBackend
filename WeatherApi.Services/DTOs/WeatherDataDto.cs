using System;

namespace WeatherApi.Services.DTOs
{
    public class WeatherDataDto
    {
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double Precipitation { get; set; }
        public WeatherWarning? Warning { get; set; }
    }
}