using Ardalis.GuardClauses;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;

namespace Weather.API.Features.Weather.GetForecast
{
    internal sealed class GetForecastWeatherHandler : IHttpRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>
    {
        private readonly IValidator<GetForecastWeatherQuery> _getForecastWeatherQueryValidator;
        private readonly IValidator<ForecastWeatherDto> _forecastWeatherValidator;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetForecastWeatherHandler> _logger;
        public GetForecastWeatherHandler(
            IValidator<GetForecastWeatherQuery> getForecastWeatherQueryValidator,
            IWeatherService weatherService,
            IValidator<ForecastWeatherDto> forecastWeatherValidator,
            ILogger<GetForecastWeatherHandler> logger)
        {
            _getForecastWeatherQueryValidator = Guard.Against.Null(getForecastWeatherQueryValidator);
            _weatherService = Guard.Against.Null(weatherService);
            _forecastWeatherValidator = Guard.Against.Null(forecastWeatherValidator);
            _logger = Guard.Against.Null(logger);
        }
        public async Task<HttpDataResponse<ForecastWeatherDto>> HandleAsync(GetForecastWeatherQuery request, CancellationToken cancellationToken)
        {
            if (!_getForecastWeatherQueryValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<ForecastWeatherDto>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var forecastResult = await _weatherService.GetForecastWeather(request.Location, cancellationToken);

            if (forecastResult.IsFailed)
            {
                _logger.LogError(LogEvents.ForecastWeathersGet, forecastResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(ErrorMessages.ExternalApiError);
            }

            var validationResult = _forecastWeatherValidator.Validate(forecastResult.Value);
            if (validationResult.AnyErrors)
            {
                _logger.LogError(LogEvents.ForecastWeathersValidation, ErrorLogMessages.ValidationErrorLog, validationResult.ToString());
                return HttpDataResponses.AsInternalServerError<ForecastWeatherDto>(ErrorMessages.ExternalApiError);
            }

            return HttpDataResponses.AsOK(forecastResult.Value);
        }
    }
}
