using Validot;
using Weather.API.Domain.Validation;

namespace Weather.API.Features.Weather.GetForecast
{
    internal sealed class ForecastWeatherDtoSpecificationHolder : ISpecificationHolder<ForecastWeatherDto>
    {
        public Specification<ForecastWeatherDto> Specification { get; }
        public ForecastWeatherDtoSpecificationHolder()
        {
            Specification<double> tempSpecification = s => s
                .Rule(GeneralPredicates.isValidTemperature);

            Specification<DateTime> dateTimeSpecification = s => s
                .Rule(s => s > DateTime.Now.AddDays(-1));

            Specification<ForecastTemperatureDto> forecastTemperatureSpecification = s => s
                .Member(m => m.Temperature, tempSpecification)
                .Member(m => m.DateTime, dateTimeSpecification);

            Specification<ForecastWeatherDto> forecastSpecification = s => s
                .Member(m => m.ForecastTemperatures, m => m.AsCollection(forecastTemperatureSpecification))
                .Member(m => m.CityName, m => m.NotEmpty().NotWhiteSpace());

            Specification = forecastSpecification;
        }
    }

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
