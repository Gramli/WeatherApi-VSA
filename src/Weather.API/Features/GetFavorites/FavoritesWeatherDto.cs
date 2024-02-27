using Weather.API.Domain.Dtos;

namespace Weather.API.Features.Favorites.GetFavorites
{
    public sealed class FavoritesWeatherDto
    {
        public IReadOnlyCollection<CurrentWeatherDto> FavoriteWeathers { get; init; } = new List<CurrentWeatherDto>();
    }
}
