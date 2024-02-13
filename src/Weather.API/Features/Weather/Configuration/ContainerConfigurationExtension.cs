using Validot;
using Weather.API.Features.Weather.Abstractions;
using Weather.API.Features.Weather.Dtos;
using Weather.API.Features.Weather.Queries;
using Weather.API.Features.Weather.Services;
using Weather.API.Features.Weather.Validation;
using Weather.API.Shared.Dtos;
using Weather.API.Shared.Extensions;

namespace Weather.API.Features.Weather.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddWeather(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddHandlers()
                .AddServices();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IGetCurrentWeatherHandler, GetCurrentWeatherHandler>()
                .AddScoped<IGetForecastWeatherHandler, GetForecastWeatherHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQuerySpecificationHolder, GetCurrentWeatherQuery>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>();
        }

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IWeatherService, WeatherService>();
        }
    }
}
