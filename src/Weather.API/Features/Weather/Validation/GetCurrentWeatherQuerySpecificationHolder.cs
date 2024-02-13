using Validot;
using Weather.API.Features.Weather.Queries;
using WeatherApi.Shared.Validation;

namespace Weather.API.Features.Weather.Validation
{
    internal sealed class GetCurrentWeatherQuerySpecificationHolder : ISpecificationHolder<GetCurrentWeatherQuery>
    {
        public Specification<GetCurrentWeatherQuery> Specification { get; }

        public GetCurrentWeatherQuerySpecificationHolder()
        {
            Specification<GetCurrentWeatherQuery> getCurrentWeatherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = getCurrentWeatherQuerySpecification;
        }
    }
}
