using Microsoft.AspNetCore.Mvc;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Weather.GetCurrent;
using WeatherApi.Domain.Http;

namespace Weather.API.EndpointBuilders
{
    public static class GetCurrentEndpointBuilder
    {
        public static IEndpointRouteBuilder BuildGetCurrentWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/current",
                async (long latitude, long longitude, [FromServices] IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude,longitude), cancellationToken))
                        .Produces<DataResponse<CurrentWeatherDto>>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }
    }
}
