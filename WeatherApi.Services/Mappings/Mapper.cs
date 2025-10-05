using AutoMapper;
using WeatherApi.Services.DTOs;
using WeatherApi.Services.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<NasaWeatherResponse, WeatherDataDto>();
    }
}