using Weather.API.Configuration;
using Weather.API.EndpointBuilders;
using Weather.API.Features.Favorites;
using Weather.API.Features.Favorites.AddFavorites;
using Weather.API.Features.Favorites.GetFavorites;
using Weather.API.Features.Weather;
using Weather.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain(builder.Configuration);
builder.Services.AddWeather();
builder.Services.AddFavorites();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app
    .MapGroup("weather")
    .BuildAddFavoriteWeatherEndpoints()
    .BuildGetFavoriteWeatherEndpoints()
    .BuildGetForecastWeatherEndpoints()
    .BuildGetCurrentWeatherEndpoints();


app.Run();