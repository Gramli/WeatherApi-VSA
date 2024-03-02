using Validot;

namespace Weather.API.Features.DeleteFavorites
{
    internal sealed class DeleteFavoriteCommandSpecificationHolder : ISpecificationHolder<DeleteFavoriteCommand>
    {
        public Specification<DeleteFavoriteCommand> Specification { get; }

        public DeleteFavoriteCommandSpecificationHolder()
        {
            Specification<DeleteFavoriteCommand> addFavoriteCommandSpecification = s => s
                .Member(m => m.Id, r => r.NonNegative());

            Specification = addFavoriteCommandSpecification;
        }
    }
}
