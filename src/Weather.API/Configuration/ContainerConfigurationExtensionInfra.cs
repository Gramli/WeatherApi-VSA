using Microsoft.EntityFrameworkCore;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using Weather.API.Domain.Service;
using Weather.API.Domain.Validation;
using Wheaterbit.Client.Configuration;

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
                .AddServices();
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

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IWeatherService, WeatherService>();
        }
    }
}
