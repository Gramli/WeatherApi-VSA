using FluentResults;
using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Favorites.Abstractions
{
    public interface IFavoritesQueriesRepository
    {
        Task<Result<IEnumerable<LocationDto>>> GetFavorites(CancellationToken cancellationToken);
    }
}
