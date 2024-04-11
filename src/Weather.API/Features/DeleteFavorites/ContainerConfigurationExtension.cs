using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Extensions;
using Validot;
using Weather.API.Domain.Extensions;

namespace Weather.API.Features.DeleteFavorites
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildDeleteFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapDelete("v1/favorite/{id}",
                async (int id, [FromServices] IHttpRequestHandler<bool, DeleteFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new DeleteFavoriteCommand { Id = id }, cancellationToken))
                        .ProducesDataResponse<bool>()
                        .WithName("DeleteFavorite")
                        .WithTags("Delete");

            return endpointRouteBuilder;
        }

        public static IServiceCollection AddDeleteFavorites(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IHttpRequestHandler<bool, DeleteFavoriteCommand>, DeleteFavoriteHandler>()
                .AddValidotSingleton<IValidator<DeleteFavoriteCommand>, DeleteFavoriteCommandSpecificationHolder, DeleteFavoriteCommand>();

    }
}
