using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Weather.API.Features.Favorites.Abstractions;
using Weather.API.Features.Favorites.Commands;
using Weather.API.Features.Favorites.Entities;
using Weather.API.Shared.Database.EFContext;

namespace Weather.API.Features.Favorites.Repositories
{
    internal sealed class FavoritesCommandsRepository : IFavoritesCommandsRepository
    {
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;
        public FavoritesCommandsRepository(WeatherContext weatherContext, IMapper mapper)
        {
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }

        public async Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken)
        {
            var locationEntity = _mapper.Map<FavoriteLocationEntity>(addFavoriteCommand.Location);
            await _weatherContext.FavoriteLocations.AddAsync(locationEntity);
            await _weatherContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(locationEntity.Id);
        }
    }
}
