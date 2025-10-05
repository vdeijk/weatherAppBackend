
namespace WeatherApi.Services.DTOs
{
    public class FestivalDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? Description { get; set; }
    }
}