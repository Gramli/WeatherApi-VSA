using Validot;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Validation;

namespace Weather.API.Features.Weather.GetCurrent
{
    internal sealed class CurrentWeatherDtoSpecificationHolder : ISpecificationHolder<CurrentWeatherDto>
    {
        public Specification<CurrentWeatherDto> Specification { get; }

        public CurrentWeatherDtoSpecificationHolder()
        {
            Specification<string> timeStringSpecification = s => s
                .NotEmpty()
                .And()
                .Rule(m => DateTime.TryParse(m, out var _));

            Specification<double> tempSpecification = s => s
                .Rule(GeneralPredicates.isValidTemperature);

            Specification<CurrentWeatherDto> currentWeatherDtoSpecification = s => s
                .Member(m => m.Sunrise, timeStringSpecification)
                .Member(m => m.Sunset, timeStringSpecification)
                .Member(m => m.Temperature, tempSpecification)
                .Member(m => m.CityName, m => m.NotEmpty().NotWhiteSpace());

            Specification = currentWeatherDtoSpecification;
        }
    }

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
