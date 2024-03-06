using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.Entities;
using Weather.API.Features.DeleteFavorites;
using Weather.API.UnitTests.Domain.Database;


namespace Weather.API.UnitTests.Features.DeleteFavorites
{
    public class DeleteFavoriteHandlerTests
    {
        private readonly Mock<IValidator<DeleteFavoriteCommand>> _deleteFavoriteCommandValidatorMock;
        private readonly Mock<TestWeatherContext> _weatherContextMock;
        private readonly Mock<DbSet<FavoriteLocationEntity>> _favoriteLocationEntityDbSetMock;

        private readonly IRequestHandler<bool, DeleteFavoriteCommand> _uut;
        public DeleteFavoriteHandlerTests()
        {
            _deleteFavoriteCommandValidatorMock = new();
            var loggerMock = new Mock<ILogger<DeleteFavoriteHandler>>();
            _weatherContextMock = new();
            _favoriteLocationEntityDbSetMock = new();

            _uut = new DeleteFavoriteHandler(_deleteFavoriteCommandValidatorMock.Object, loggerMock.Object, _weatherContextMock.Object);
        }

        [Fact]
        public async Task InvalidRequest()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 1 };

            _deleteFavoriteCommandValidatorMock.Setup(x => x.IsValid(deleteFavoriteCommand)).Returns(false);

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Failed()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 1 };

            _deleteFavoriteCommandValidatorMock.Setup(x => x.IsValid(deleteFavoriteCommand)).Returns(true);

            _favoriteLocationEntityDbSetMock.Setup(x => x.FindAsync(It.IsAny<int>(), CancellationToken.None)).ThrowsAsync(new DbUpdateException());
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 1 };

            _deleteFavoriteCommandValidatorMock.Setup(x => x.IsValid(deleteFavoriteCommand)).Returns(true);

            var favoriteLocation = new FavoriteLocationEntity();
            _favoriteLocationEntityDbSetMock.Setup(x => x.FindAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(favoriteLocation);
            _favoriteLocationEntityDbSetMock.Setup(x => x.Remove(favoriteLocation));
            _weatherContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.True(result.Data);
        }

    }
}
