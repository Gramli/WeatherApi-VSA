using Microsoft.EntityFrameworkCore;
using Weather.API.Shared.Database.EFContext;
using Wheaterbit.Client.Configuration;

namespace Weather.API.Shared.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddShared(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddDatabase()
                .AddExternalHttpServices(configuration);
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
    }
}
