using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Favorites.Commands
{
    public sealed class AddFavoriteCommand
    {
        public LocationDto Location { get; init; } = new LocationDto();
    }
}
