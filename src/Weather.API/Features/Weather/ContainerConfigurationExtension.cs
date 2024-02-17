using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Weather.GetCurrent;
using Weather.API.Features.Weather.GetForecast;

namespace Weather.API.Features.Weather
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddWeather(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddHandlers()
                .AddMapping();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>, GetCurrentWeatherHandler>()
                .AddScoped<IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>, GetForecastWeatherHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQuerySpecificationHolder, GetCurrentWeatherQuery>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>();
        }

        private static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(ForecastProfile));
        }
    }
}
