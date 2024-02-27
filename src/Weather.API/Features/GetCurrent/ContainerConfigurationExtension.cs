using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;

namespace Weather.API.Features.Weather.GetCurrent
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddGetCurrentWeather(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddHandlers();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>, GetCurrentWeatherHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQuerySpecificationHolder, GetCurrentWeatherQuery>();
        }
    }
}
