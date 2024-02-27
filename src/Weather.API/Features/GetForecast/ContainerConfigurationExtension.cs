using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;

namespace Weather.API.Features.Weather.GetForecast
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddGetForecastWeather(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddHandlers()
                .AddMapping();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>, GetForecastWeatherHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>();
        }

        private static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(ForecastProfile));
        }
    }
}
