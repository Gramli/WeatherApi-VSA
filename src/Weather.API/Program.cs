using Weather.API.EndpointBuilders;
using Weather.API.Features.Favorites.Configuration;
using Weather.API.Features.Favorites.EndpointBuilders;
using Weather.API.Features.Weather.Configuration;
using Weather.API.Middleware;
using Weather.API.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShared(builder.Configuration);
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

app.BuildWeatherEndpoints()
    .BuildFavoriteEndpoints();

app.Run();