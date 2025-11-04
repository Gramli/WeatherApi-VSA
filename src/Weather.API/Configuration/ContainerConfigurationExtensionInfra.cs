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
        public static IServiceCollection AddDomain(this IServiceCollection serviceCollection, IConfigurationSection weatherbitConfiguration)
        {
            return serviceCollection
                .AddDatabase()
                .AddExternalHttpServices(weatherbitConfiguration)
                .AddValidation()
                .AddLogging()
                .AddServices();
        }

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<WeatherContext>(opt => opt.UseInMemoryDatabase("Weather"));
        }

        private static IServiceCollection AddExternalHttpServices(this IServiceCollection serviceCollection, IConfigurationSection weatherbitConfiguration)
        {
            return serviceCollection
                .AddHttpClient()
                .AddWeatherbit(weatherbitConfiguration);
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

        public static IServiceCollection SetXRapidKeyEnvironmentVariable(this IServiceCollection serviceCollection, IConfigurationSection weatherbitConfiguration)
        {
            var apiKey = Environment.GetEnvironmentVariable("XRapidAPIKey");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("XRapidAPIKey is not configured. Set it via user-secrets (dev) or environment variable (prod).");
            }

            weatherbitConfiguration["XRapidAPIKey"] = apiKey;

            return serviceCollection;
        }
    }
}
