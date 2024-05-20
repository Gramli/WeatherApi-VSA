using Ardalis.GuardClauses;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using Validot;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Database.Entities;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;

namespace Weather.API.Features.Favorites.AddFavorites
{
    internal sealed class AddFavoriteHandler : IHttpRequestHandler<int, AddFavoriteCommand>
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

        public async Task<HttpDataResponse<int>> HandleAsync(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_addFavoriteCommandValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<int>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var addResult = await AddFavoriteLocationSafeAsync(request, cancellationToken);
            if (addResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<int>("Location was not stored in database.");
            }

            return HttpDataResponses.AsOK(addResult.Value);
        }

        public async Task<Result<int>> AddFavoriteLocationSafeAsync(AddFavoriteCommand addFavoriteCommand, CancellationToken cancellationToken)
        {
            var locationEntity = _mapper.Map<FavoriteLocationEntity>(addFavoriteCommand.Location);
            try
            {
                await _weatherContext.FavoriteLocations.AddAsync(locationEntity);
                await _weatherContext.SaveChangesAsync(cancellationToken);
                return Result.Ok(locationEntity.Id);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, ex, "Can't store location into database.");
                return Result.Fail(ex.Message);
            }
        }
    }
}
