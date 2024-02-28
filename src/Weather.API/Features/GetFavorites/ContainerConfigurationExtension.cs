using Microsoft.AspNetCore.Mvc;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using Weather.API.Features.AddFavorites;
using Weather.API.Features.Favorites.GetFavorites;
using Weather.API.Features.GetFavorites;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.GetFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildGetFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/favorites",
                async ([FromServices] IRequestHandler<FavoritesWeatherDto, EmptyRequest> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .Produces<DataResponse<FavoritesWeatherDto>>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }

        public static IServiceCollection AddGetFavorites(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddScoped<IRequestHandler<FavoritesWeatherDto, EmptyRequest>, GetFavoritesHandler>();
    }
}
