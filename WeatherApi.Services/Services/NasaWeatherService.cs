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
                    // Randomly select one of three mock weather scenarios
                    var dateKey = date.ToString("yyyyMMdd");
                    var scenarios = new[]
                    {
                        // Normal weather (no warning)
                        $"{{\n  \"Type\": \"Feature\",\n  \"Geometry\": {{\n    \"Type\": \"Point\",\n    \"Coordinates\": [4.8952, 52.3702, 0.0]\n  }},\n  \"Properties\": {{\n    \"Location\": \"Amsterdam\",\n    \"Parameter\": {{\n      \"Values\": {{\n        \"T2M\": {{ \"{dateKey}\": 15.2 }},\n        \"PRECTOTCORR\": {{ \"{dateKey}\": 0.3 }},\n        \"WS2M\": {{ \"{dateKey}\": 5.1 }}\n      }}\n    }}\n  }}\n}}",
                        // Heavy rain (triggers HeavyRain warning)
                        $"{{\n  \"Type\": \"Feature\",\n  \"Geometry\": {{\n    \"Type\": \"Point\",\n    \"Coordinates\": [4.8952, 52.3702, 0.0]\n  }},\n  \"Properties\": {{\n    \"Location\": \"Amsterdam\",\n    \"Parameter\": {{\n      \"Values\": {{\n        \"T2M\": {{ \"{dateKey}\": 12.0 }},\n        \"PRECTOTCORR\": {{ \"{dateKey}\": 15.0 }},\n        \"WS2M\": {{ \"{dateKey}\": 6.0 }}\n      }}\n    }}\n  }}\n}}",
                        // High wind (triggers HighWind warning)
                        $"{{\n  \"Type\": \"Feature\",\n  \"Geometry\": {{\n    \"Type\": \"Point\",\n    \"Coordinates\": [4.8952, 52.3702, 0.0]\n  }},\n  \"Properties\": {{\n    \"Location\": \"Amsterdam\",\n    \"Parameter\": {{\n      \"Values\": {{\n        \"T2M\": {{ \"{dateKey}\": 17.0 }},\n        \"PRECTOTCORR\": {{ \"{dateKey}\": 0.2 }},\n        \"WS2M\": {{ \"{dateKey}\": 18.0 }}\n      }}\n    }}\n  }}\n}}"
                    };
                    var selected = scenarios[_staticRandom.Next(scenarios.Length)];
                    return selected;
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