using WeatherApi.Domain.Http;

namespace Weather.API.Domain.Abstractions
{
    public interface IRequestHandler<TResponse, in TRequest>
    {
        Task<HttpDataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
