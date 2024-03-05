using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Validot;
using Validot.Results;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;
using Weather.API.Features.Weather.GetCurrent;
using Weather.API.UnitTests.TestExtensions;

namespace Weather.API.UnitTests.Features.GetCurrent
{
    public class GetCurrentWeatherHandlerTests
    {
        private readonly Mock<IValidator<GetCurrentWeatherQuery>> _getCurrentWeatherQueryValidatorMock;
        private readonly Mock<IValidator<CurrentWeatherDto>> _currentWeatherValidatorMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<GetCurrentWeatherHandler>> _loggerMock;

        private readonly IRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery> _uut;
        public GetCurrentWeatherHandlerTests()
        {
            _getCurrentWeatherQueryValidatorMock = new();
            _currentWeatherValidatorMock = new();
            _weatherServiceMock = new();
            _loggerMock = new();

            _uut = new GetCurrentWeatherHandler(_getCurrentWeatherQueryValidatorMock.Object, _currentWeatherValidatorMock.Object, _weatherServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);

            _getCurrentWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetCurrentWeatherQuery>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_Failed()
        {
            //Arrange
            var errorMessage = "error";
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);

            _getCurrentWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetCurrentWeatherQuery>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(errorMessage));
            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.ExternalApiError, result.Errors.Single());
            Assert.Null(result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(getCurrentWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.CurrentWeathersGet, errorMessage, Times.Once());
        }

        [Fact]
        public async Task CurrentWeather_ValidationFailed()
        {
            //Arrange
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);
            var currentWeather = new CurrentWeatherDto();

            var validationResutlMock = new Mock<IValidationResult>();
            validationResutlMock.SetupGet(x => x.AnyErrors).Returns(true);
            _getCurrentWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetCurrentWeatherQuery>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            _currentWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<CurrentWeatherDto>(), It.IsAny<bool>())).Returns(validationResutlMock.Object);

            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(getCurrentWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.Validate(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather)), It.Is<bool>(y => !y)), Times.Once);
            validationResutlMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.CurrentWeathersValidation, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);
            var currentWeather = new CurrentWeatherDto();

            var validationResutlMock = new Mock<IValidationResult>();
            validationResutlMock.SetupGet(x => x.AnyErrors).Returns(false);
            _getCurrentWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetCurrentWeatherQuery>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            _currentWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<CurrentWeatherDto>(), It.IsAny<bool>())).Returns(validationResutlMock.Object);

            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Equal(currentWeather, result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(getCurrentWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.Validate(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather)), It.Is<bool>(y => !y)), Times.Once);
            validationResutlMock.VerifyGet(x => x.AnyErrors, Times.Once);
        }
    }
}
