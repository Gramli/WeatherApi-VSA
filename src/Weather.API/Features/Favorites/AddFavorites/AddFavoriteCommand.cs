using Weather.API.Domain.Dtos;

namespace Weather.API.Features.Favorites.AddFavorites
{
    internal sealed class AddFavoriteCommand
    {
        public LocationDto Location { get; init; } = new LocationDto();
    }
}
