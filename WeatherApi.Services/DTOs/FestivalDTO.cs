
namespace WeatherApi.Services.DTOs
{
    public class FestivalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}