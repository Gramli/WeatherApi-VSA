using Microsoft.AspNetCore.Mvc;
using Weather.API.Features.Weather.Abstractions;
using Weather.API.Features.Weather.Dtos;
using Weather.API.Features.Weather.Queries;
using Weather.API.Shared.Dtos;
using Weather.API.Shared.Extensions;
using WeatherApi.Shared.Http;

namespace Weather.API.EndpointBuilders
{
    public static class WeatherBuilder
    {
        public static IEndpointRouteBuilder BuildWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder
                .MapGroup("weather")
                .BuildActualWeatherEndpoints()
                .BuildForecastWeatherEndpoints();

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildActualWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/current",
                async (long latitude, long longtitude, [FromServices] IGetCurrentWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude,longtitude), cancellationToken))
                        .Produces<DataResponse<CurrentWeatherDto>>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/forecast",
                async (long latitude, long longtitude, [FromServices] IGetForecastWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longtitude), cancellationToken))
                        .Produces<DataResponse<ForecastWeatherDto>>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }
    }
}
