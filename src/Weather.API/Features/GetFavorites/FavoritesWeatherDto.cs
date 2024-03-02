using Weather.API.Features.GetFavorites;

namespace Weather.API.Features.Favorites.GetFavorites
{
    public sealed class FavoritesWeatherDto
    {
        public IReadOnlyCollection<FavoriteCurrentWeatherDto> FavoriteWeathers { get; init; } = new List<FavoriteCurrentWeatherDto>();
    }
}
