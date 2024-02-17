using AutoMapper;

namespace Weather.API.Features.Weather.GetForecast
{
    internal sealed class ForecastProfile : Profile
    {
        public ForecastProfile()
        {
            CreateMap<Wheaterbit.Client.Dtos.ForecastTemperatureDto, ForecastTemperatureDto>()
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.temp))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.datetime));

            CreateMap<Wheaterbit.Client.Dtos.ForecastWeatherDto, ForecastWeatherDto>()
                .ForMember(dest => dest.ForecastTemperatures, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.city_name));
        }
    }
}
