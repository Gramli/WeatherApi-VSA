using AutoMapper;
using Weather.API.Features.Weather.Dtos;
using Weather.API.Shared.Dtos;

namespace Weather.Infrastructure.Mapping.Profiles
{
    internal sealed class ExternalClientsProfile : Profile
    {
        public ExternalClientsProfile()
        {
            CreateMap<Wheaterbit.Client.Dtos.CurrentWeatherDto, CurrentWeatherDto>()
                .ForMember(dest=>dest.Temperature, opt => opt.MapFrom(src => src.temp))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.ob_time))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city_name))
                .ForMember(dest => dest.Sunrise, opt => opt.MapFrom(src => src.sunrise))
                .ForMember(dest => dest.Sunset, opt => opt.MapFrom(src => src.sunset));

            CreateMap<Wheaterbit.Client.Dtos.ForecastTemperatureDto, ForecastTemperatureDto>()
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.temp))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.datetime));

            CreateMap<Wheaterbit.Client.Dtos.ForecastWeatherDto, ForecastWeatherDto>()
                .ForMember(dest => dest.ForecastTemperatures, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city_name));
        }
    }
}
