using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Validot;
using Weather.API.Domain.Abstractions;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Database.Entities;
using Weather.API.Domain.Extensions;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;
using WeatherApi.Domain.Http;

namespace Weather.API.Features.Favorites.AddFavorites
{
    internal sealed class AddFavoriteHandler : IRequestHandler<bool, AddFavoriteCommand>
    {
        private readonly IMapper _mapper;
        private readonly WeatherContext _weatherContext;
        private readonly IValidator<AddFavoriteCommand> _addFavoriteCommandValidator;
        private readonly ILogger<AddFavoriteHandler> _logger;
        public AddFavoriteHandler(
            IValidator<AddFavoriteCommand> addFavoriteCommandValidator, 
            ILogger<AddFavoriteHandler> logger,
            WeatherContext weatherContext, 
            IMapper mapper)
        {
            _addFavoriteCommandValidator = Guard.Against.Null(addFavoriteCommandValidator);
            _logger = Guard.Against.Null(logger);
            _weatherContext = Guard.Against.Null(weatherContext);
            _mapper = Guard.Against.Null(mapper);
        }

        public async Task<HttpDataResponse<bool>> HandleAsync(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_addFavoriteCommandValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<bool>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var addResult = await AddFavoriteLocation(request, cancellationToken);
            if (addResult.IsFailed)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, addResult.Errors.JoinToMessage());
                return HttpDataResponses.AsInternalServerError<bool>(ErrorMessages.CantStoreLocation);
            }

            return HttpDataResponses.AsOK(true);
        }

        public async Task<Result<int>> AddFavoriteLocation(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken)
        {
            var locationEntity = _mapper.Map<FavoriteLocationEntity>(addFavoriteCommand.Location);
            await _weatherContext.FavoriteLocations.AddAsync(locationEntity);
            await _weatherContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(locationEntity.Id);
        }
    }
}
