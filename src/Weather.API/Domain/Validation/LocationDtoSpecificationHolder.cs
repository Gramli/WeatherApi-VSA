using Validot;
using Weather.API.Domain.Dtos;

namespace Weather.API.Domain.Validation
{
    internal sealed class LocationDtoSpecificationHolder : ISpecificationHolder<LocationDto>
    {
        public Specification<LocationDto> Specification { get; }

        public LocationDtoSpecificationHolder()
        {
            Specification = GeneralPredicates.isValidLocation;
        }
    }
}
