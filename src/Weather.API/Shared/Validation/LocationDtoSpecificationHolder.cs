using Validot;
using Weather.API.Shared.Dtos;

namespace WeatherApi.Shared.Validation
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
