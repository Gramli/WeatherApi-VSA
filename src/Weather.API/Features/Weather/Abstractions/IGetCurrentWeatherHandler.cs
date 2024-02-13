using Weather.API.Features.Weather.Queries;
using Weather.API.Shared.Abstractions;
using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Weather.Abstractions
{
    public interface IGetCurrentWeatherHandler : IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>
    {
    }
}
