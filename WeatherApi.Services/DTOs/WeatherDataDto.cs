using System;

namespace WeatherApi.Services.DTOs
{
    public class WeatherDataDto
    {
    // Location removed: not provided by NASA API
        public double Temperature { get; set; }
        public string? Condition { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
    // Icon removed: not provided by NASA API
        public double Precipitation { get; set; }
    }
}