using Validot;
using Weather.Core.Validation;
using Weather.Domain.Dtos.Queries;

namespace Weather.Core.Features.Weather.Validation
{
    internal sealed class GetCurrentWeatcherQuerySpecificationHolder : ISpecificationHolder<GetCurrentWeatherQuery>
    {
        public Specification<GetCurrentWeatherQuery> Specification { get; }

        public GetCurrentWeatcherQuerySpecificationHolder()
        {
            Specification<GetCurrentWeatherQuery> getCurrentWeatcherQuerySpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = getCurrentWeatcherQuerySpecification;
        }
    }
}
