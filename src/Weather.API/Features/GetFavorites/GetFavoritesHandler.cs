using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Favorites.GetFavorites
{
    internal sealed class GetFavoritesHandler : IRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetFavoritesHandler> _logger;
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;

        public GetFavoritesHandler(
            IWeatherService weatherService,
            IValidator<LocationDto> locationValidator,
            IValidator<CurrentWeatherDto> currentWeatherValidator,
            ILogger<GetFavoritesHandler> logger,
            WeatherContext weatherContext,
            IMapper mapper)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _logger = Guard.Against.Null(logger);
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }

        public async Task<HttpDataResponse<FavoritesWeatherDto>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var favoriteLocations = await GetFavoritesAync(cancellationToken);

            if (!favoriteLocations.HasAny())
            {
                return HttpDataResponses.AsNoContent<FavoritesWeatherDto>();
            }

            return await GetFavoritesAsync(favoriteLocations, cancellationToken);

        }

        private async Task<HttpDataResponse<FavoritesWeatherDto>> GetFavoritesAsync(IEnumerable<LocationDto> favoriteLocationsResult, CancellationToken cancellationToken)
        {
            var result = new List<CurrentWeatherDto>();
            var errorMessages = new List<string>();

            await favoriteLocationsResult.ForEachAsync(async (location) =>
            {
                var favoriteWeather = await GetWeatherAsync(location, cancellationToken);

                if (favoriteWeather.IsFailed)
                {
                    errorMessages.AddRange(favoriteWeather.Errors.ToErrorMessages());
                    return;
                }

                result.Add(favoriteWeather.Value);
            });

            return result.Any() ?
                HttpDataResponses.AsOK(new FavoritesWeatherDto { FavoriteWeathers = result, }, errorMessages) :
                HttpDataResponses.AsInternalServerError<FavoritesWeatherDto>(errorMessages);
        }

        private async Task<Result<CurrentWeatherDto>> GetWeatherAsync(LocationDto location, CancellationToken cancellationToken)
        {
            if (!_locationValidator.IsValid(location))
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, ErrorLogMessages.InvalidLocation, location);
                return Result.Fail(ErrorMessages.RequestValidationError);
            }

            var favoriteWeather = await _weatherService.GetCurrentWeather(location, cancellationToken);
            if (favoriteWeather.IsFailed)
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, favoriteWeather.Errors.JoinToMessage());
                return Result.Fail(ErrorMessages.ExternalApiError);
            }

            if (!_currentWeatherValidator.IsValid(favoriteWeather.Value))
            {
                _logger.LogWarning(LogEvents.FavoriteWeathersGeneral, ErrorLogMessages.InvalidWeather, location);
                return Result.Fail(ErrorMessages.ExternalApiError);
            }

            return favoriteWeather.Value;
        }

        public async Task<IEnumerable<LocationDto>> GetFavoritesAync(CancellationToken cancellationToken)
        {
            var favoriteLocationEntities = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);
            return _mapper.Map<List<LocationDto>>(favoriteLocationEntities);
        }
    }
}
