using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Favorites.AddFavorites;
using Weather.API.Features.Favorites.GetFavorites;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Favorites
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddFavorites(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidation()
                .AddHandlers();

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection) => serviceCollection
                .AddScoped<IRequestHandler<FavoritesWeatherDto, EmptyRequest>, GetFavoritesHandler>()
                .AddScoped<IRequestHandler<bool, AddFavoriteCommand>, AddFavoriteHandler>();

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>();
    }
}
