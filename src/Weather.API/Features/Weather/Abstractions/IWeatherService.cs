using FluentResults;
using Weather.API.Shared.Dtos;
using Weather.Domain.Dtos;

namespace Weather.API.Features.Weather.Abstractions
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken);

        Task<Result<ForecastWeatherDto>> GetForecastWeather(LocationDto locationDto, CancellationToken cancellationToken);
    }
}
