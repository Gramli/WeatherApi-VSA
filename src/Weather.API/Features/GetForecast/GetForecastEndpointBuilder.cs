using Microsoft.AspNetCore.Mvc;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Weather.GetForecast
{
    public static class GetForecastEndpointBuilder
    {
        public static IEndpointRouteBuilder BuildGetForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/forecast",
                async (long latitude, long longitude, [FromServices] IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longitude), cancellationToken))
                        .Produces<DataResponse<ForecastWeatherDto>>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }
    }
}
