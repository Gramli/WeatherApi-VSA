using Microsoft.EntityFrameworkCore;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Database.Entities;

namespace Weather.API.UnitTests.Domain.Database
{
    public class TestWeatherContext : WeatherContext
    {
        public TestWeatherContext()
            : base(new DbContextOptions<WeatherContext>())
        {
        }

        public override DbSet<FavoriteLocationEntity>? FavoriteLocations { get; }
    }
}
