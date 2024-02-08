using Weather.API.Features.Favorites.Dtos;
using Weather.API.Shared.Abstractions;
using WeatherApi.Shared.Http;

namespace Weather.API.Features.Favorites.Abstractions
{
    public interface IGetFavoritesHandler : IRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
    }
}
