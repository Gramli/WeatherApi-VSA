using Validot;
using Weather.API.Features.Favorites.Abstractions;
using Weather.API.Features.Favorites.Commands;
using Weather.API.Features.Favorites.Queries;
using Weather.API.Features.Favorites.Repositories;
using Weather.API.Shared.Extensions;
using Weather.Core.Features.Favorites.Commands;
using Weather.Core.Features.Favorites.Validation;

namespace Weather.API.Features.Favorites.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddFavorites(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidation()
                .AddHandlers()
                .AddDatabase();

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
    }
}
