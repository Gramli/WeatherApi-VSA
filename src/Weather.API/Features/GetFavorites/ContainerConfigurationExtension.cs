using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Extensions;
using Weather.API.Features.Favorites.GetFavorites;

namespace Weather.API.Features.GetFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildGetFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/favorites",
                async ([FromServices] IHttpRequestHandler<FavoritesWeatherDto, EmptyRequest> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .ProducesDataResponse<FavoritesWeatherDto>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }

        public static IServiceCollection AddGetFavorites(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddScoped<IHttpRequestHandler<FavoritesWeatherDto, EmptyRequest>, GetFavoritesHandler>();
    }
}
