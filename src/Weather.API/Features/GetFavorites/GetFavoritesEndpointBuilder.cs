using Microsoft.AspNetCore.Mvc;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Favorites.GetFavorites
{
    public static class GetFavoritesEndpointBuilder
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
    }
}
