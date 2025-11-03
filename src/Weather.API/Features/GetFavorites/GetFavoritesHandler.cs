using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Database.Entities;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;
using Weather.API.Features.GetFavorites;

namespace Weather.API.Features.Favorites.GetFavorites
{
    internal sealed class GetFavoritesHandler : IHttpRequestHandler<FavoritesWeatherDto, EmptyRequest>
    {
        private readonly IValidator<LocationDto> _locationValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetFavoritesHandler> _logger;
        private readonly WeatherContext _weatherContext;

        public GetFavoritesHandler(
            IWeatherService weatherService,
            IValidator<LocationDto> locationValidator,
            IValidator<CurrentWeatherDto> currentWeatherValidator,
            ILogger<GetFavoritesHandler> logger,
            WeatherContext weatherContext)
        {
            _locationValidator = Guard.Against.Null(locationValidator);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _logger = Guard.Against.Null(logger);
            _weatherContext = Guard.Against.Null(weatherContext);
        }

        public async Task<HttpDataResponse<FavoritesWeatherDto>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var favoriteLocations = await _weatherContext.FavoriteLocations.ToListAsync(cancellationToken);

            if (!favoriteLocations.HasAny())
            {
                return HttpDataResponses.AsNoContent<FavoritesWeatherDto>();
            }

            return await GetFavoritesAsync(favoriteLocations, cancellationToken);

        }

        private async Task<HttpDataResponse<FavoritesWeatherDto>> GetFavoritesAsync(IEnumerable<FavoriteLocationEntity> favoriteLocationsResult, CancellationToken cancellationToken)
        {
            var result = new List<FavoriteCurrentWeatherDto>();
            var errorMessages = new List<string>();

            await favoriteLocationsResult.ForEachAsync(async (location) =>
            {
                var favoriteWeather = await GetWeatherAsync(new LocationDto 
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                }, cancellationToken);

                if (favoriteWeather.IsFailed)
                {
                    errorMessages.AddRange(favoriteWeather.Errors.ToErrorMessages());
                    return;
                }

                result.Add(new FavoriteCurrentWeatherDto
                {
                    CityName = favoriteWeather.Value.CityName,
                    DateTime = favoriteWeather.Value.DateTime,
                    Sunrise = favoriteWeather.Value.Sunrise,
                    Sunset = favoriteWeather.Value.Sunset,
                    Id = location.Id,
                    Temperature = favoriteWeather.Value.Temperature
                });
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
            return favoriteLocationEntities.Select(favoriteLocationEntities => new LocationDto
            {
                Latitude = favoriteLocationEntities.Latitude,
                Longitude = favoriteLocationEntities.Longitude,
            });
        }
    }
}
