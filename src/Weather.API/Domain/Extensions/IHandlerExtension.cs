﻿using Weather.API.Domain.Abstractions;
using WeatherApi.Domain.Http;

namespace Weather.API.Domain.Extensions
{
    internal static class IHandlerExtension
    {
        internal static async Task<IResult> SendAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken)
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            return Results.Json(new DataResponse<TResponse> { Data = response.Data, Errors = response.Errors }, statusCode: (int)response.StatusCode);
        }
    }
}
