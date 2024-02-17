using AutoMapper;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Database.Entities;

namespace Weather.API.Domain.Mapping
{
    internal sealed class WeatherEntitiesProfile : Profile
    {
        public WeatherEntitiesProfile()
        {
            CreateMap<LocationDto, FavoriteLocationEntity>();
            CreateMap<FavoriteLocationEntity, LocationDto>();
        }
    }
}
