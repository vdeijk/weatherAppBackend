namespace WeatherApi.Services.Interfaces
{
    public interface INasaWeatherService
    {
        Task<string> GetWeatherDataAsync(double latitude, double longitude, DateTime date);
    }
}