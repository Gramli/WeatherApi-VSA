using Weather.API.Features.Weather.Dtos;
using Weather.API.Features.Weather.Queries;
using Weather.API.Shared.Abstractions;

namespace Weather.API.Features.Weather.Abstractions
{
    public interface IGetForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>
    {
    }
}
