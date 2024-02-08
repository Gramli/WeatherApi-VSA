using Microsoft.EntityFrameworkCore;
using Weather.API.Features.Favorites.Entities;

namespace Weather.API.Shared.Database.EFContext
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options) { }

        public virtual DbSet<FavoriteLocationEntity> FavoriteLocations => Set<FavoriteLocationEntity>();
    }
}
