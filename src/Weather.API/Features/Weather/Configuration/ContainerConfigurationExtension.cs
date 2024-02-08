using Microsoft.Extensions.DependencyInjection;
using Validot;
using Weather.API.Features.Favorites.Abstractions;
using Weather.API.Features.Favorites.Mapping;
using Weather.API.Features.Favorites.Queries;
using Weather.API.Features.Weather.Abstractions;
using Weather.API.Features.Weather.Services;
using Weather.API.Shared.Dtos;
using Weather.API.Shared.Extensions;
using Weather.Core.Features.Favorites.Commands;
using Weather.Core.Features.Favorites.Validation;
using Weather.Core.Features.Weather.Queries;
using Weather.Core.Features.Weather.Validation;
using Weather.Core.Validation;
using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Commands;
using Weather.Domain.Dtos.Queries;

namespace Weather.API.Features.Weather.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddHandlers();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IGetCurrentWeatherHandler, GetCurrentWeatherHandler>()
                .AddScoped<IGetFavoritesHandler, GetFavoritesHandler>()
                .AddScoped<IGetForecastWeatherHandler, GetForecastWeatherHandler>()
                .AddScoped<IAddFavoriteHandler, AddFavoriteHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<LocationDto>, LocationDtoSpecificationHolder, LocationDto>()
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatcherQuerySpecificationHolder, GetCurrentWeatherQuery>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>();
        }

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IWeatherService, WeatherService>();
        }

        private static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(WeatherEntitiesProfile));
        }
    }
}
