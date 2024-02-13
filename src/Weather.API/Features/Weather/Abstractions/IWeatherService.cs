using FluentResults;
using Weather.API.Features.Weather.Dtos;
using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Weather.Abstractions
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken);

        Task<Result<ForecastWeatherDto>> GetForecastWeather(LocationDto locationDto, CancellationToken cancellationToken);
    }
}
