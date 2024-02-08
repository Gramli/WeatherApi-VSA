using Weather.API.Features.Favorites.Commands;
using Weather.API.Shared.Abstractions;

namespace Weather.API.Features.Favorites.Abstractions
{
    public interface IAddFavoriteHandler : IRequestHandler<bool, AddFavoriteCommand>
    {

    }
}
