using Microsoft.AspNetCore.Mvc;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.DeleteFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildDeleteFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapDelete("v1/favorite/{id}",
                async (int id, [FromServices] IRequestHandler<bool, DeleteFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new DeleteFavoriteCommand { Id = id }, cancellationToken))
                        .Produces<DataResponse<bool>>()
                        .WithName("DeleteFavorite")
                        .WithTags("Delete");

            return endpointRouteBuilder;
        }

        public static IServiceCollection AddDeleteFavorites(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IRequestHandler<bool, DeleteFavoriteCommand>, DeleteFavoriteHandler>()
                .AddValidotSingleton<IValidator<DeleteFavoriteCommand>, DeleteFavoriteCommandSpecificationHolder, DeleteFavoriteCommand>();

    }
}
