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

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
else
{
    builder.Configuration["XRapidAPIKey"] = Environment.GetEnvironmentVariable("XRapidAPIKey");
}

builder.AddLogging();

builder.Services.AddOpenApi();

builder.Services.AddDomain(builder.Configuration);
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