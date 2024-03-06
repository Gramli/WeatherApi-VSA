using Weather.API.Configuration;
using Weather.API.Features.AddFavorites;
using Weather.API.Features.DeleteFavorites;
using Weather.API.Features.Favorites.AddFavorites;
using Weather.API.Features.Favorites.GetFavorites;
using Weather.API.Features.GetFavorites;
using Weather.API.Features.Weather.GetCurrent;
using Weather.API.Features.Weather.GetForecast;
using Weather.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain(builder.Configuration);
builder.Services
    .AddAddFavorites()
    .AddGetFavorites()
    .AddGetCurrentWeather()
    .AddGetForecastWeather()
    .AddDeleteFavorites();

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
    .BuildGetCurrentWeatherEndpoints()
    .BuildDeleteFavoriteWeatherEndpoints();

app.Run();