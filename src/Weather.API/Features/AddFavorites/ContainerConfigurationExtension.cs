using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Extensions;
using Validot;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Favorites.AddFavorites;

namespace Weather.API.Features.AddFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildAddFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("/favorites",
                async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IHttpRequestHandler<int, AddFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .ProducesDataResponse<int>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            return endpointRouteBuilder;
        }

        public static IServiceCollection AddAddFavorites(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddScoped<IHttpRequestHandler<int, AddFavoriteCommand>, AddFavoriteHandler>()
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>();

    }
}
