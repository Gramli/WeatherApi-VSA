using WeatherApi.Shared.Http;

namespace Weather.API.Shared.Abstractions
{
    public interface IRequestHandler<TResponse, in TRequest>
    {
        Task<HttpDataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
