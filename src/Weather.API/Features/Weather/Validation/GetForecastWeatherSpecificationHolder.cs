using Validot;
using Weather.Core.Validation;
using Weather.Domain.Dtos.Queries;

namespace Weather.Core.Features.Weather.Validation
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
