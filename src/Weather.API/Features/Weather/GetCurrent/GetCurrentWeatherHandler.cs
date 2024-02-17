using Ardalis.GuardClauses;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Weather.GetCurrent
{
    internal sealed class GetCurrentWeatherHandler : IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>
    {
        private readonly IValidator<GetCurrentWeatherQuery> _getCurrentWeatherQueryValidator;
        private readonly IValidator<CurrentWeatherDto> _currentWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetCurrentWeatherHandler> _logger;
        public GetCurrentWeatherHandler(IValidator<GetCurrentWeatherQuery> getCurrentWeatherQueryValidator,
            IValidator<CurrentWeatherDto> currentWeatherValidator,
            IWeatherService weatherService,
            ILogger<GetCurrentWeatherHandler> logger)
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
