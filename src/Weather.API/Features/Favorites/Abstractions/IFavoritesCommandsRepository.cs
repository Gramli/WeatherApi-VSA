using FluentResults;
using Weather.API.Features.Favorites.Commands;

namespace Weather.API.Features.Favorites.Abstractions
{
    public interface IFavoritesCommandsRepository
    {
        Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken);
    }
}
