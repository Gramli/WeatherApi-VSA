using Microsoft.AspNetCore.Mvc;
using Weather.API.Features.Favorites.Abstractions;
using Weather.API.Features.Favorites.Commands;
using Weather.API.Features.Favorites.Dtos;
using Weather.API.Shared.Extensions;
using WeatherApi.Shared.Http;

namespace Weather.API.Features.Favorites.EndpointBuilders
{
    public static class FavoritesBuilder
    {
        public static IEndpointRouteBuilder BuildFavoriteEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder
                .MapGroup("weather")
                .BuildFavoriteWeatherEndpoints();

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/favorites",
                async ([FromServices] IGetFavoritesHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .Produces<DataResponse<FavoritesWeatherDto>>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            endpointRouteBuilder.MapPost("v1/favorite",
                async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IAddFavoriteHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .Produces<DataResponse<bool>>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            return endpointRouteBuilder;
        }
    }
}
