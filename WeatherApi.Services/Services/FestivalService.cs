using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WeatherApi.Services.DTOs;
using WeatherApi.Services.Interfaces;

namespace WeatherApi.Services
{
    public class FestivalService : IFestivalService
    {
        public bool UseMockData { get; set; } = true;
        private readonly string _mockFileName = "mock-festival-data.json";

        /// <summary>
        /// Gets all festivals from mock data or external source.
        /// </summary>
        /// <returns>Enumerable of FestivalDto.</returns>
        public IEnumerable<FestivalDto> GetAllFestivals()
        {
            try
            {
                if (UseMockData)
                {
                    var mockFilePath = Path.Combine(AppContext.BaseDirectory, _mockFileName);
                    if (!File.Exists(mockFilePath))
                        throw new FileNotFoundException("Mock festival data file not found.", mockFilePath);

                    var mockJson = File.ReadAllText(mockFilePath);
                    var festivals = JsonSerializer.Deserialize<List<FestivalDto>>(mockJson);
                    return festivals ?? new List<FestivalDto>();
                }
                // External API call or actual implementation would go here
                return new List<FestivalDto>();
            }
            catch (Exception ex)
            {
                // Log the exception (could use a logger if available)
                Console.Error.WriteLine($"Error in GetAllFestivals: {ex}");
                return new List<FestivalDto>();
            }
        }
    }
}
