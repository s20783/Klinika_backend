using FluentValidation;

namespace Application.WizytaLeki.Queries
{
    public class VisitMedicamentListQueryValidator : AbstractValidator<VisitMedicamentListQuery>
    {
        public VisitMedicamentListQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}