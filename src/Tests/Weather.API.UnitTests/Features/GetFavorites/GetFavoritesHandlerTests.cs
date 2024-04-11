using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using System.Net;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.Entities;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Logging;
using Weather.API.Features.Favorites.GetFavorites;
using Weather.API.UnitTests.Domain.Database;
using Weather.API.UnitTests.TestExtensions;

namespace Weather.API.UnitTests.Features.GetFavorites
{
    public class GetFavoritesHandlerTests
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<GetFavoritesHandler>> _loggerMock;
        private readonly Mock<IValidator<LocationDto>> _locationValidatorMock;
        private readonly Mock<IValidator<CurrentWeatherDto>> _currentWeatherValidatorMock;
        private readonly Mock<TestWeatherContext> _weatherContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<DbSet<FavoriteLocationEntity>> _favoriteLocationEntityDbSetMock;

        private readonly IHttpRequestHandler<FavoritesWeatherDto, EmptyRequest> _uut;
        public GetFavoritesHandlerTests()
        {
            _weatherServiceMock = new();
            _loggerMock = new();
            _locationValidatorMock = new();
            _currentWeatherValidatorMock = new();
            _weatherContextMock = new();
            _mapperMock = new();
            _favoriteLocationEntityDbSetMock = new();

            _uut = new GetFavoritesHandler(
                _weatherServiceMock.Object,
                _locationValidatorMock.Object,
                _currentWeatherValidatorMock.Object,
                _loggerMock.Object,
                _weatherContextMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetFavorites_Empty()
        {
            //Arrange
            _favoriteLocationEntityDbSetMock.SetupMock([]);

            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var favoriteLocations = new List<FavoriteLocationEntity>(){ new() };
            _favoriteLocationEntityDbSetMock.SetupMock(favoriteLocations);
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);
            _mapperMock.Setup(x => x.Map<List<LocationDto>>(favoriteLocations)).Returns(new List<LocationDto> { new LocationDto() });
            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _locationValidatorMock.Verify(x => x.IsValid(It.IsAny<LocationDto>()), Times.Once);
        }

        [Fact]
        public async Task EmptyResult_GetCurrentWeather_Fail()
        {
            //Arrange
            var failMessage = "Some fail message";
            var locationDto = new LocationDto { Latitude = 1, Longitude = 1 };

            var favoriteLocations = new List<FavoriteLocationEntity>() { new() };
            _favoriteLocationEntityDbSetMock.SetupMock(favoriteLocations);
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);
            _mapperMock.Setup(x => x.Map<LocationDto>(It.IsAny<FavoriteLocationEntity>())).Returns(locationDto);
            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Warning, LogEvents.FavoriteWeathersGeneral, failMessage, Times.Once());
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
        }

        [Fact]
        public async Task One_Of_GetCurrentWeather_Failed()
        {
            //Arrange
            var failMessage = "Some fail message";
            var locationDto = new LocationDto { Latitude = 1, Longitude = 1 };

            var favoriteLocations = new List<FavoriteLocationEntity> { new(), new FavoriteLocationEntity { Latitude = locationDto.Latitude, Longitude = locationDto.Longitude } };
            _favoriteLocationEntityDbSetMock.SetupMock(favoriteLocations);
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);
            _mapperMock.Setup(x => x.Map<LocationDto>(It.Is<FavoriteLocationEntity>(y=> y.Latitude!= locationDto.Latitude))).Returns(new LocationDto());
            _mapperMock.Setup(x => x.Map<LocationDto>(It.Is<FavoriteLocationEntity>(y => y.Latitude == locationDto.Latitude))).Returns(locationDto);
            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);

            var currentWeather = new CurrentWeatherDto();

            _currentWeatherValidatorMock.Setup(x => x.IsValid(It.IsAny<CurrentWeatherDto>())).Returns(true);

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.Is<LocationDto>(y => !y.Equals(locationDto)), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.FavoriteWeathers);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _loggerMock.VerifyLog(LogLevel.Warning, LogEvents.FavoriteWeathersGeneral, failMessage, Times.Once());
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.IsValid(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather))), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_Validation_Fail()
        {
            //Arrange
            var locationDto = new LocationDto { Latitude = 1, Longitude = 1 };

            var favoriteLocations = new List<FavoriteLocationEntity>() { new() };
            _favoriteLocationEntityDbSetMock.SetupMock(favoriteLocations);
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);
            _mapperMock.Setup(x => x.Map<LocationDto>(It.IsAny<FavoriteLocationEntity>())).Returns(locationDto);

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _currentWeatherValidatorMock.Setup(x => x.IsValid(It.IsAny<CurrentWeatherDto>())).Returns(false);
            var currentWeather = new CurrentWeatherDto();

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.IsValid(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather))), Times.Once);
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var locationDto = new LocationDto { Latitude = 1, Longitude = 1 };

            var favoriteLocations = new List<FavoriteLocationEntity>() { new() };
            _favoriteLocationEntityDbSetMock.SetupMock(favoriteLocations);
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);
            _mapperMock.Setup(x => x.Map<LocationDto>(It.IsAny<FavoriteLocationEntity>())).Returns(locationDto);

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _currentWeatherValidatorMock.Setup(x => x.IsValid(It.IsAny<CurrentWeatherDto>())).Returns(true);
            var currentWeather = new CurrentWeatherDto();

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.FavoriteWeathers);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.IsValid(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather))), Times.Once);
        }
    }
}
