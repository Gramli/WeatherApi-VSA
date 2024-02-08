using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Favorites.Dtos
{
    public sealed class FavoritesWeatherDto
    {
        public IReadOnlyCollection<CurrentWeatherDto> FavoriteWeathers { get; init; } = new List<CurrentWeatherDto>();
    }
}
