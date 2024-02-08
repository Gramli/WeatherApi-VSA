using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Queries;

namespace Weather.API.Features.Weather.Abstractions
{
    public interface IGetForecastWeatherHandler : IRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>
    {
    }
}
