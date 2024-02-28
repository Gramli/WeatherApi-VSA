using Weather.API.Domain.Dtos;

namespace Weather.API.Features.Weather.GetCurrent
{
    public sealed class GetCurrentWeatherQuery
    {
        public LocationDto Location { get; init; }
        public GetCurrentWeatherQuery(double latitude, double longitude)
        {
            Location = new LocationDto
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }
    }
}
