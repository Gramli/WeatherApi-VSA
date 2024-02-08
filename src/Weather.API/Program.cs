using Weather.API.Configuration;
using Weather.API.EndpointBuilders;
using Weather.API.Features.Weather.Configuration;
using Weather.API.Middleware;
using Weather.API.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();

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

app.BuildWeatherEndpoints();

app.Run();