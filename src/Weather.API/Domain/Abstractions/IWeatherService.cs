using FluentResults;
using Weather.API.Domain.Dtos;
using Weather.API.Features.Weather.GetForecast;

namespace Weather.API.Domain.Abstractions
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken);

        Task<Result<ForecastWeatherDto>> GetForecastWeather(LocationDto locationDto, CancellationToken cancellationToken);
    }
}
