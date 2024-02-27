using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Favorites.AddFavorites;

namespace Weather.API.Features.AddFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddAddFavorites(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidation()
                .AddHandlers();

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection) => serviceCollection
                .AddScoped<IRequestHandler<bool, AddFavoriteCommand>, AddFavoriteHandler>();

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) => serviceCollection
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>();
    }
}
