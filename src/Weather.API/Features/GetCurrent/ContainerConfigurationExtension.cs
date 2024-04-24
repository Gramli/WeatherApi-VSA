using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Extensions;
using Validot;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;

namespace Weather.API.Features.Weather.GetCurrent
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildGetCurrentWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("current",
                async (double latitude, double longitude, [FromServices] IHttpRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude, longitude), cancellationToken))
                        .ProducesDataResponse<CurrentWeatherDto>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        public static IServiceCollection AddGetCurrentWeather(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IHttpRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>, GetCurrentWeatherHandler>()
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQuerySpecificationHolder, GetCurrentWeatherQuery>();
        
    }
}
