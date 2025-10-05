# NASA Space Apps Challenge: Weather App Backend

## Challenge Context
This backend is designed for the NASA Space Apps Challenge. It demonstrates how to build a robust, scalable weather API using NASA POWER data. The solution:
- Uses NASA’s open meteorological data model for compatibility and scientific accuracy.
- Provides endpoints for temperature, precipitation, wind speed, humidity, and derived weather conditions.
- Supports mock data for development and demonstration when the NASA API is offline, ensuring reliability.
- Is ready for real-world deployment and easy integration with frontend applications.

# Weather App Backend Documentation

## Overview
This project is a backend API for a weather application, designed for the NASA Space Apps Challenge. It is built with .NET and structured for modularity, testability, and easy integration with NASA POWER data.

## Features
- Provides weather data (temperature, precipitation, wind speed, humidity, and derived condition) for a given location and date.
- Uses mock data that matches the NASA POWER API response format when the real API is unavailable.
- DTOs and mapping logic are designed to be compatible with both mock and real NASA data.
- Easily switch between mock and live data sources.

## Data Model
The API returns weather data in the following format:

```json
{
  "temperature": 15.2,
  "condition": "Clear",
  "humidity": 65,
  "windSpeed": 5.1,
  "precipitation": 0.3
}
```

## Mock Data Usage
When the NASA POWER API is offline, the backend loads mock data from `WeatherApi.Services/Data/mock-nasa-weather.json`. This file is structured to match the real API response, ensuring seamless transition when the API is available.

### Example Mock Data File
```json
{
  "Type": "Feature",
  "Geometry": {
    "Type": "Point",
    "Coordinates": [4.8952, 52.3702, 0.0]
  },
  "Properties": {
    "Parameter": {
      "Values": {
        "T2M": { "20251005": 15.2 },
        "PRECTOTCORR": { "20251005": 0.3 },
        "WS2M": { "20251005": 5.1 },
        "RH2M": { "20251005": 65 }
      }
    }
  }
}
```

- The mock data structure is identical to the real NASA API response.
- To switch to live data, set `UseMockData = false` in the service configuration.
- Update the mock file as needed to test different scenarios.

## Switching to Real NASA Data
- Set `UseMockData = false` in the service configuration.
- Ensure the NASA API endpoint and parameters are correct.
- The backend will then fetch live data from NASA POWER.

## How to Run
1. Clone the repository.
2. Build the solution with `dotnet build`.
3. Run the backend with `dotnet run --project WeatherApi/WeatherApi.csproj`.
4. The API will be available at `http://localhost:5002` (default).

## Extending the App
- To add new weather parameters, update the DTO, mapping logic, and mock data file.
- To support additional locations or dates, expand the mock data or connect to the live NASA API.

## Why Mock Data?
- NASA POWER API may be offline or rate-limited during the challenge.
- Mock data enables full development, testing, and demonstration.
- The mock data structure is identical to the real API, so your code remains compatible.

## Contact & Questions
For questions, improvements, or feedback, see code comments or contact the project maintainer.

---

**NASA Space Apps Challenge Submission**
This backend is designed to meet the requirements of the challenge, ensuring reliability and compatibility with NASA data standards—even when the real API is unavailable.
