using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;
using SmallApiToolkit.Extensions;
using SmallApiToolkit.Middleware;
using Weather.API.Configuration;
using Weather.API.Features.AddFavorites;
using Weather.API.Features.DeleteFavorites;
using Weather.API.Features.GetFavorites;
using Weather.API.Features.Weather.GetCurrent;
using Weather.API.Features.Weather.GetForecast;
using Wheaterbit.Client.Options;

var builder = WebApplication.CreateBuilder(args);

var weatherbitconfig = builder.Configuration.GetSection(WeatherbitOptions.Weatherbit);

if (!builder.Environment.IsDevelopment())
{
    builder.Services.SetXRapidKeyEnvironmentVariable(weatherbitconfig);
}

builder.AddLogging();

builder.Services.AddOpenApi();

builder.Services.AddDomain(weatherbitconfig);
builder.Services
    .AddAddFavorites()
    .AddGetFavorites()
    .AddGetCurrentWeather()
    .AddGetForecastWeather()
    .AddDeleteFavorites();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Title = "Weather API";
        options.Theme = ScalarTheme.Mars;
        options.ShowSidebar = true;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app
    .MapGroup("weather")
    .MapVersionGroup(1)
    .BuildAddFavoriteWeatherEndpoints()
    .BuildGetFavoriteWeatherEndpoints()
    .BuildGetForecastWeatherEndpoints()
    .BuildGetCurrentWeatherEndpoints()
    .BuildDeleteFavoriteWeatherEndpoints();

app.Run();