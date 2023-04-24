using FluentValidation;

namespace Application.WeterynarzSpecjalizacje.Queries
{
    public class VetSpecializationListQueryValidator : AbstractValidator<VetSpecializationListQuery>
    {
        public VetSpecializationListQueryValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
