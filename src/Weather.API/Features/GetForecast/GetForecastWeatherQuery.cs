using Weather.API.Domain.Dtos;

namespace Weather.API.Features.Weather.GetForecast
{
    public sealed class GetForecastWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetForecastWeatherQuery(long latitude, long longitude)
        {
            Location = new LocationDto
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }
    }
}
