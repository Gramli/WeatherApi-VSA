using Validot;
using Weather.API.Features.Favorites.Abstractions;
using Weather.API.Features.Favorites.Commands;
using Weather.API.Features.Favorites.Mapping;
using Weather.API.Features.Favorites.Queries;
using Weather.API.Features.Favorites.Repositories;
using Weather.API.Features.Favorites.Validation;
using Weather.API.Shared.Extensions;

namespace Weather.API.Features.Favorites.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddFavorites(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidation()
                .AddHandlers()
                .AddDatabase()
                .AddMapping();

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection) => serviceCollection
                .AddScoped<IGetFavoritesHandler, GetFavoritesHandler>()
                .AddScoped<IAddFavoriteHandler, AddFavoriteHandler>();

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>();

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IFavoritesQueriesRepository, FavoritesQueriesRepository>()
                .AddScoped<IFavoritesCommandsRepository, FavoritesCommandsRepository>();
        }

        private static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAutoMapper(typeof(WeatherEntitiesProfile));
        }
    }
}
