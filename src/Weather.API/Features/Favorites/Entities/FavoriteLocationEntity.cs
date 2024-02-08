namespace Weather.API.Features.Favorites.Entities;

public sealed class FavoriteLocationEntity
{
    public int Id { get; set; }
    public long Latitude { get; set; }
    public long Longitude { get; set; }
}
