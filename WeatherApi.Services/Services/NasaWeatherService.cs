using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using WeatherApi.Services.Interfaces;

namespace WeatherApi.Services
{
    public class NasaWeatherService : INasaWeatherService
    {
        public bool UseMockData { get; set; } = true;
        private readonly HttpClient _httpClient;

        public NasaWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets weather data from NASA POWER API or mock data.
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees.</param>
        /// <param name="longitude">Longitude in decimal degrees.</param>
        /// <param name="date">Date for weather data.</param>
        /// <returns>Raw NASA weather data as JSON string.</returns>
        private static readonly Random _staticRandom = new Random();

        public async Task<string> GetWeatherDataAsync(double latitude, double longitude, DateTime date)
        {
            try
            {
                if (UseMockData)
                {
                    // Randomly select one of several mock JSON files
                    var mockFiles = new[]
                    {
                        "mock-weather-normal.json",
                        "mock-weather-heavy-rain.json",
                        "mock-weather-high-wind.json"
                    };
                    var selectedFile = mockFiles[_staticRandom.Next(mockFiles.Length)];
                    var mockFilePath = Path.Combine(AppContext.BaseDirectory, "WeatherApi.Services", "Data", selectedFile);
                    if (!File.Exists(mockFilePath))
                        throw new FileNotFoundException($"Mock weather data file not found: {selectedFile}", mockFilePath);
                    var mockJson = await File.ReadAllTextAsync(mockFilePath);
                    return mockJson;
                }

                string dateStr = date.ToString("yyyyMMdd");
                string latStr = latitude.ToString(CultureInfo.InvariantCulture);
                string lonStr = longitude.ToString(CultureInfo.InvariantCulture);
                string url = $"https://power.larc.nasa.gov/api/temporal/daily/point?parameters=T2M,PRECTOTCORR,WS2M&start={dateStr}&end={dateStr}&latitude={latStr}&longitude={lonStr}&format=JSON&community=RE";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "WeatherApp/1.0");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"NASA API error: {response.StatusCode}. Details: {error}");
                }
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new HttpRequestException("NASA API returned empty response.");
                }
                return content;
            }
            catch (Exception ex)
            {
                // Log the exception (could use a logger if available)
                Console.Error.WriteLine($"Error in GetWeatherDataAsync: {ex}");
                throw;
            }
        }
    }
}