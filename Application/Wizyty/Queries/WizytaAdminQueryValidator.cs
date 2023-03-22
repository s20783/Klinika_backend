using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class WizytaAdminQueryValidator : AbstractValidator<WizytaAdminQuery>
    {
        public WizytaAdminQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}