using Microsoft.AspNetCore.Mvc;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Favorites.AddFavorites
{
    public static class AddFavoritesEndpointBuilder
    {
        public static IEndpointRouteBuilder BuildAddFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("v1/favorite",
                async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IRequestHandler<bool, AddFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .Produces<DataResponse<bool>>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            return endpointRouteBuilder;
        }
    }
}
