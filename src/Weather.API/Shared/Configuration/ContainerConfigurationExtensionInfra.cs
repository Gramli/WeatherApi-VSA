using Microsoft.EntityFrameworkCore;
using Validot;
using Weather.API.Shared.Database.EFContext;
using Weather.API.Shared.Dtos;
using Weather.API.Shared.Extensions;
using WeatherApi.Shared.Validation;
using Wheaterbit.Client.Configuration;

namespace Weather.API.Shared.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddShared(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddDatabase()
                .AddExternalHttpServices(configuration)
                .AddValidation()
                .AddLogging();
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
    }
}
