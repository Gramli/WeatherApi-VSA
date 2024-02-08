using AutoMapper;
using Weather.API.Features.Favorites.Entities;
using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Favorites.Mapping
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
