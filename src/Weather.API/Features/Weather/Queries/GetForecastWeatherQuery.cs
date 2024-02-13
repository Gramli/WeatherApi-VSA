using Weather.API.Shared.Dtos;

namespace Weather.API.Features.Weather.Queries
{
    public sealed class GetForecastWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetForecastWeatherQuery(long latitude, long longtitude)
        {
            Location = new LocationDto
            {
                Latitude = latitude,
                Longitude = longtitude
            };
        }
    }
}
