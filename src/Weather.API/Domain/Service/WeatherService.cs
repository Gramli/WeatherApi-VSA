using Ardalis.GuardClauses;
using FluentResults;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Wheaterbit.Client.Abstractions;
using Weather.API.Domain.Extensions;
using Weather.API.Features.Weather.GetForecast;
using Weather.API.Domain.Resources;

namespace Weather.API.Domain.Service
{
    internal sealed class WeatherService : IWeatherService
    {
        private readonly IWeatherbitHttpClient _weatherbitHttpClient;

        public WeatherService(IWeatherbitHttpClient weatherbitHttpClient)
        {
            _weatherbitHttpClient = Guard.Against.Null(weatherbitHttpClient);
        }

        public async Task<Result<CurrentWeatherDto>> GetCurrentWeather(LocationDto locationDto, CancellationToken cancellationToken)
        {
            var currentWeatherResult = await _weatherbitHttpClient.GetCurrentWeather(locationDto.Latitude, locationDto.Longitude, cancellationToken);
            if (currentWeatherResult.IsFailed)
            {
                return Result.Fail(currentWeatherResult.Errors);
            }

            if (currentWeatherResult.Value is null || !currentWeatherResult.Value.Data.HasAny())
            {
                return Result.Fail(ServiceErrorMessages.ExternalClientGetDataFailed_EmptyOrNull);
            }

            if (currentWeatherResult.Value.Data.Count != 1)
            {
                return Result.Fail(string.Format(ServiceErrorMessages.ExternalClientGetDataFailed_CorruptedData_InvalidCount, currentWeatherResult.Value.Data.Count));
            }

            var item = currentWeatherResult.Value.Data.Single();

            return Result.Ok(
            
                new CurrentWeatherDto
                {
                    CityName = item.city_name,
                    DateTime = item.ob_time,
                    Temperature = item.temp,
                    Sunrise = item.sunrise,
                    Sunset = item.sunset,
                }
            );
        }

        public async Task<Result<ForecastWeatherDto>> GetForecastWeather(LocationDto locationDto, CancellationToken cancellationToken)
        {
            var forecastWeatherResult = await _weatherbitHttpClient.GetSixteenDayForecast(locationDto.Latitude, locationDto.Longitude, cancellationToken);
            if (forecastWeatherResult.IsFailed)
            {
                return Result.Fail(forecastWeatherResult.Errors);
            }

            if (forecastWeatherResult.Value is null || !forecastWeatherResult.Value.Data.Any())
            {
                return Result.Fail(ServiceErrorMessages.ExternalClientGetDataFailed_EmptyOrNull);
            }

            return Result.Ok(new ForecastWeatherDto
            {
                CityName = forecastWeatherResult.Value.city_name,
                ForecastTemperatures = forecastWeatherResult.Value.Data.Select(item => new ForecastTemperatureDto
                {
                    DateTime = item.datetime,
                    Temperature = item.temp,
                }).ToList()
            });
        }
    }
}
