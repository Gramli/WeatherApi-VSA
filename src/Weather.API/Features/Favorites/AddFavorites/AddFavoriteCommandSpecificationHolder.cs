using Validot;
using Weather.API.Domain.Validation;

namespace Weather.API.Features.Favorites.AddFavorites
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
