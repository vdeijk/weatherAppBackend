
using WeatherApi.Services.DTOs;

namespace WeatherApi.Services.Interfaces
{
    public interface IFestivalService
    {
        IEnumerable<FestivalDto> GetAllFestivals();
    }
}
