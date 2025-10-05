# Weather API Mock Data Usage

## Why Mock Data?
The NASA POWER API is currently offline, making it impossible to fetch real weather data. To ensure development, testing, and demonstration can continue, this project uses mock data files that closely match the NASA API's data model and structure.

## How Mock Data Works
- The backend loads mock data from JSON files (e.g., `mock-nasa-weather.json`).
- The mock data structure is designed to match the real NASA API response, so the code and data models remain compatible.
- When the NASA API becomes available, switching to live data requires minimal changes.

## Mock Data File Example
`WeatherApi.Services/Data/mock-nasa-weather.json`:
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

## Switching to Real NASA Data
- Set `UseMockData = false` in the service configuration.
- Ensure the NASA API endpoint and parameters are correct.
- Remove or archive the mock data files if no longer needed.

## Benefits
- Enables development and testing when the real API is unavailable.
- Keeps code and data models aligned with NASA standards.
- Easy transition to live data when the API is online.

## Additional Notes
- All DTOs and mapping logic are designed to work with both mock and real NASA data.
- If you add new fields to the DTO, update the mock data and mapping logic accordingly.

---

For questions or updates, see the code comments or contact the project maintainer.
