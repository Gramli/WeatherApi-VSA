using Ardalis.GuardClauses;
using Validot;
using Weather.API.Features.Weather.Abstractions;
using Weather.API.Resources;
using Weather.API.Shared.Dtos;
using Weather.API.Shared.Extensions;
using Weather.API.Shared.Resources;
using WeatherApi.Shared.Http;
using WeatherApi.Shared.Logging;

namespace Weather.API.Features.Weather.Queries
{
    internal sealed class GetCurrentWeatherHandler : IGetCurrentWeatherHandler
    {
        private readonly IValidator<GetCurrentWeatherQuery> _getCurrentWeatherQueryValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<IGetCurrentWeatherHandler> _logger;
        public GetCurrentWeatherHandler(IValidator<GetCurrentWeatherQuery> getCurrentWeatherQueryValidator,
            IValidator<CurrentWeatherDto> currentWeatherValidator,
            IWeatherService weatherService,
            ILogger<IGetCurrentWeatherHandler> logger)
        {
            _getCurrentWeatherQueryValidator = Guard.Against.Null(getCurrentWeatherQueryValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _currentWeatherValidator = Guard.Against.Null(currentWeatherValidator);
            _logger = Guard.Against.Null(logger);
        }
        public async Task<HttpDataResponse<CurrentWeatherDto>> HandleAsync(GetCurrentWeatherQuery request, CancellationToken cancellationToken)
        {
            if (!_getCurrentWeatherQueryValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<CurrentWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var getCurrentWeatherResult = await _weatherService.GetCurrentWeather(request.Location, cancellationToken);
            if (getCurrentWeatherResult.IsFailed)
            {
                _logger.LogError(LogEvents.CurrentWeathersGet, getCurrentWeatherResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(ErrorMessages.ExternalApiError);
            }

            var validationResult = _currentWeatherValidator.Validate(getCurrentWeatherResult.Value);
            if (validationResult.AnyErrors)
            {
                _logger.LogError(LogEvents.CurrentWeathersValidation, ErrorLogMessages.ValidationErrorLog, validationResult.ToString());
                return HttpDataResponses.AsInternalServerError<CurrentWeatherDto>(ErrorMessages.ExternalApiError);
            }

            return HttpDataResponses.AsOK(getCurrentWeatherResult.Value);
        }
    }
}
