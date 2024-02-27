using Weather.API.Domain.Abstractions;
using Weather.API.Features.AddFavorites;
using Weather.API.Features.Favorites.GetFavorites;
using Weather.API.Features.GetFavorites;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.GetFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddGetFavorites(this IServiceCollection serviceCollection) => serviceCollection
                .AddHandlers();

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection) => serviceCollection
                .AddScoped<IRequestHandler<FavoritesWeatherDto, EmptyRequest>, GetFavoritesHandler>();
    }
}
