using Microsoft.AspNetCore.Mvc;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Extensions;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Weather.GetCurrent
{
    public static class ContainerConfigurationExtension
    {
        public static IEndpointRouteBuilder BuildGetCurrentWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/current",
                async (double latitude, double longitude, [FromServices] IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude, longitude), cancellationToken))
                        .Produces<DataResponse<CurrentWeatherDto>>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        public static IServiceCollection AddGetCurrentWeather(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>, GetCurrentWeatherHandler>()
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQuerySpecificationHolder, GetCurrentWeatherQuery>();
        
    }
}
