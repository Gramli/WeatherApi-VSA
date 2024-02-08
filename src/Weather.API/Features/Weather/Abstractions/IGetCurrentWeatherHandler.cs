using Weather.API.Shared.Dtos;
using Weather.Core.Queries;
using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Queries;

namespace Weather.API.Features.Weather.Abstractions
{
    public interface IGetCurrentWeatherHandler : IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>
    {
    }
}
