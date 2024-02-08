using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Weather.API.Features.Favorites.Abstractions;
using Weather.API.Shared.Database.EFContext;
using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Favorites.Repositories
{
    internal sealed class FavoritesQueriesRepository : IFavoritesQueriesRepository
    {
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;
        public FavoritesQueriesRepository(WeatherContext weatherContext, IMapper mapper)
        {
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }
        public async Task<Result<IEnumerable<LocationDto>>> GetFavorites(CancellationToken cancellationToken)
        {
            var facoriteLocationEntities = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);
            var resultData = _mapper.Map<List<LocationDto>>(facoriteLocationEntities);
            return Result.Ok((IEnumerable<LocationDto>)resultData);
        }
    }
}
