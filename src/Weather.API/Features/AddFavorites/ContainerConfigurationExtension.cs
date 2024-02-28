using Microsoft.AspNetCore.Mvc;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Favorites.AddFavorites;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.AddFavorites
{
    public static class ContainerConfigurationExtension
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

        public static IServiceCollection AddAddFavorites(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddScoped<IRequestHandler<bool, AddFavoriteCommand>, AddFavoriteHandler>()
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>();

    }
}
