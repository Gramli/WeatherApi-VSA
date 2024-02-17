using System.Net;

namespace WeatherApi.Domain.Http
{
    public class HttpDataResponse<T> : DataResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }

    }
}
