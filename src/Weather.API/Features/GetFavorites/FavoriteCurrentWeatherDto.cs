
using Weather.API.Domain.Dtos;

namespace Weather.API.Features.GetFavorites
{
    public sealed class FavoriteCurrentWeatherDto : CurrentWeatherDto
    {
        public int Id { get; init; }
    }
}
