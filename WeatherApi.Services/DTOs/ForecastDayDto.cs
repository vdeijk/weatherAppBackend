using System;

namespace WeatherApi.Services.DTOs
{
    public class ForecastDayDto
    {
        public DateTime Date { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public string? Condition { get; set; }
        public string? Icon { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
    }
}