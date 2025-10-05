using AutoMapper;
using WeatherApi.Services.DTOs;
using WeatherApi.Services.Models;

namespace WeatherApi.Services
{
    public class WeatherMappingProfile : Profile
    {
        public WeatherMappingProfile()
        {
            CreateMap<NasaWeatherResponse, WeatherDataDto>()
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => GetParameterValue(src, "T2M")))
                .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => GetParameterValue(src, "WS2M")))
                .ForMember(dest => dest.Precipitation, opt => opt.MapFrom(src => GetParameterValue(src, "PRECTOTCORR")))
                .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => "N/A"));
        }

        private static double GetParameterValue(NasaWeatherResponse src, string key)
        {
            if (src == null || src.Properties == null || src.Properties.Parameter == null || src.Properties.Parameter.Values == null)
                return key == "PRECTOTCORR" ? 0 : -999;

            var dateKey = src.Header != null ? src.Header.Start ?? src.Header.End : null;
            if (dateKey == null)
                return key == "PRECTOTCORR" ? 0 : -999;

            if (src.Properties.Parameter.Values.ContainsKey(key) &&
                src.Properties.Parameter.Values[key].ContainsKey(dateKey))
            {
                return src.Properties.Parameter.Values[key][dateKey];
            }

            return key == "PRECTOTCORR" ? 0 : -999;
        }
    }
}