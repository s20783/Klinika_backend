using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class WizytaDetailsAdminQueryValidator : AbstractValidator<WizytaDetailsAdminQuery>
    {
        public WizytaDetailsAdminQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
