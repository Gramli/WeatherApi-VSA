using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using Validot;
using Weather.API.Domain.Database.EFContext;
using Weather.API.Domain.Logging;
using Weather.API.Domain.Resources;

namespace Weather.API.Features.DeleteFavorites
{
    internal sealed class DeleteFavoriteHandler : IHttpRequestHandler<bool, DeleteFavoriteCommand>
    {
        private readonly WeatherContext _weatherContext;
        private readonly IValidator<DeleteFavoriteCommand> _validator;
        private readonly ILogger<DeleteFavoriteHandler> _logger;
        public DeleteFavoriteHandler(
            IValidator<DeleteFavoriteCommand> validator,
            ILogger<DeleteFavoriteHandler> logger,
            WeatherContext weatherContext)
        {
            _validator = Guard.Against.Null(validator);
            _logger = Guard.Against.Null(logger);
            _weatherContext = Guard.Against.Null(weatherContext);
        }

        public async Task<HttpDataResponse<bool>> HandleAsync(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_validator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<bool>(string.Format(ErrorMessages.RequestValidationError, request));
            }

            var addResult = await DeleteFavoriteLocationSafeAsync(request, cancellationToken);
            if (addResult.IsFailed)
            {
                return HttpDataResponses.AsInternalServerError<bool>("Location was not deleted from database.");
            }

            return HttpDataResponses.AsOK(true);
        }

        public async Task<Result> DeleteFavoriteLocationSafeAsync(DeleteFavoriteCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var location = await _weatherContext.FavoriteLocations.FindAsync(command.Id, cancellationToken);
                _weatherContext.Remove(location!);
                await _weatherContext.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(LogEvents.FavoriteWeathersStoreToDatabase, ex, "Can't delete location.");
                return Result.Fail(ex.Message);
            }
        }
    }
}
