using Microsoft.EntityFrameworkCore;
using Validot;
using Weather.API.Configuration;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Dtos;
using Wheaterbit.Client.Configuration;
using Weather.API.Domain.Validation;
using Weather.API.Domain.Mapping;
using Weather.API.Domain.Extensions;
using Weather.Infrastructure.Mapping.Profiles;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Service;

namespace Weather.API.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddDomain(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddDatabase()
                .AddExternalHttpServices(configuration)
                .AddValidation()
                .AddLogging()
                .AddServices()
                .AddMapping();
        }

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<WeatherContext>(opt => opt.UseInMemoryDatabase("Weather"));
        }

        private static IServiceCollection AddExternalHttpServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddHttpClient()
                .AddWeatherbit(configuration);
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidotSingleton<IValidator<LocationDto>, LocationDtoSpecificationHolder, LocationDto>();
        }

        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            return builder;
        }

        private static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(WeatherEntitiesProfile))
                .AddAutoMapper(typeof(ExternalClientsProfile));
        }

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IWeatherService, WeatherService>();
        }
    }
}
