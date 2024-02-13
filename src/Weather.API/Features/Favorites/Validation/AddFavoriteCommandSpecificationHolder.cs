using Validot;
using Weather.API.Features.Favorites.Commands;
using WeatherApi.Shared.Validation;

namespace Weather.API.Features.Favorites.Validation
{
    internal sealed class AddFavoriteCommandSpecificationHolder : ISpecificationHolder<AddFavoriteCommand>
    {
        public Specification<AddFavoriteCommand> Specification { get; }

        public AddFavoriteCommandSpecificationHolder()
        {
            Specification<AddFavoriteCommand> addFavoriteCommandSpecification = s => s
                .Member(m => m.Location, GeneralPredicates.isValidLocation);

            Specification = addFavoriteCommandSpecification;
        }
    }
}
