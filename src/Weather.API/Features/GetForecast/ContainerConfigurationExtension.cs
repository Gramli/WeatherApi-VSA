using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Extensions;
using Validot;
using Weather.API.Domain.Extensions;

namespace Weather.API.Features.Weather.GetForecast
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildGetForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/forecast",
                async (double latitude, double longitude, [FromServices] IHttpRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longitude), cancellationToken))
                        .ProducesDataResponse<ForecastWeatherDto>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }
        public static IServiceCollection AddGetForecastWeather(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IHttpRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>, GetForecastWeatherHandler>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>()
                .AddAutoMapper(typeof(ForecastProfile));
    }
}
