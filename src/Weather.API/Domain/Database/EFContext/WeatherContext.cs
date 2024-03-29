﻿using Microsoft.EntityFrameworkCore;
using Weather.API.Domain.Database.Entities;

namespace Weather.API.Domain.Database.EFContext
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options) { }

        public virtual DbSet<FavoriteLocationEntity> FavoriteLocations => Set<FavoriteLocationEntity>();
    }
}
