using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.Entities;
using Weather.API.Domain.Dtos;
using Weather.API.Domain.Logging;
using Weather.API.Features.Favorites.AddFavorites;
using Weather.API.UnitTests.Domain.Database;
using Weather.API.UnitTests.TestExtensions;

namespace Weather.API.UnitTests.Features.AddFavorites
{
    public class AddFavoriteHandlerTests
    {
        private readonly Mock<TestWeatherContext> _weatherContextMock;
        private readonly Mock<IValidator<AddFavoriteCommand>> _addFavoriteCommandValidatorMock;
        private readonly Mock<ILogger<AddFavoriteHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<DbSet<FavoriteLocationEntity>> _favoriteLocationEntityDbSetMock;

        private readonly IRequestHandler<int, AddFavoriteCommand> _uut;
        public AddFavoriteHandlerTests()
        {
            _favoriteLocationEntityDbSetMock = new();
            _weatherContextMock = new();
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);

            _addFavoriteCommandValidatorMock = new();
            _loggerMock = new();
            _mapperMock = new();

            _uut = new AddFavoriteHandler(_addFavoriteCommandValidatorMock.Object, _loggerMock.Object, _weatherContextMock.Object, _mapperMock.Object);
        }


        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };

            _addFavoriteCommandValidatorMock.Setup(x => x.IsValid(It.IsAny<AddFavoriteCommand>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            _addFavoriteCommandValidatorMock.Verify(x => x.IsValid(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand))), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteLocation_Failed()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };

            var favoriteLocationEntity = new FavoriteLocationEntity();
            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>())).Returns(favoriteLocationEntity);
            _favoriteLocationEntityDbSetMock.Setup(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>())).Throws(new DbUpdateException());
            _addFavoriteCommandValidatorMock.Setup(x => x.IsValid(It.IsAny<AddFavoriteCommand>())).Returns(true);

            //Act
            var result = await _uut.HandleAsync(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            _addFavoriteCommandValidatorMock.Verify(x => x.IsValid(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand))), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.FavoriteWeathersStoreToDatabase, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };
            var addFavoriteEntity = new FavoriteLocationEntity { Latitude = 1, Longitude = 2 };

            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(addFavoriteCommand.Location)).Returns(addFavoriteEntity);
            _addFavoriteCommandValidatorMock.Setup(x => x.IsValid(It.IsAny<AddFavoriteCommand>())).Returns(true);
            _favoriteLocationEntityDbSetMock.Setup(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()))
                .Callback(()=> { addFavoriteEntity.Id = 1; });
            _weatherContextMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).Callback(() => { addFavoriteEntity.Id = 1; });

            //Act
            var result = await _uut.HandleAsync(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            _addFavoriteCommandValidatorMock.Verify(x => x.IsValid(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand))), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
