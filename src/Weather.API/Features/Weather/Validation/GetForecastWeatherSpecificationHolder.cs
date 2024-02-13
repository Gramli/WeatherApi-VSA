using Validot;
using Weather.API.Features.Weather.Queries;
using WeatherApi.Shared.Validation;

namespace Weather.API.Features.Weather.Validation
{
    internal sealed class GetForecastWeatherSpecificationHolder : ISpecificationHolder<GetForecastWeatherQuery>
    {
        public Specification<GetForecastWeatherQuery> Specification { get; }

        public GetForecastWeatherSpecificationHolder()
        {
            Specification<GetForecastWeatherQuery> getForecastWeatherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = getForecastWeatherQuerySpecification;
        }
    }
}
