using Microsoft.AspNetCore.Mvc;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Extensions;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Weather.GetForecast
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildGetForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/forecast",
                async (double latitude, double longitude, [FromServices] IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longitude), cancellationToken))
                        .Produces<DataResponse<ForecastWeatherDto>>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }
        public static IServiceCollection AddGetForecastWeather(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>, GetForecastWeatherHandler>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>()
                .AddAutoMapper(typeof(ForecastProfile));
    }
}
