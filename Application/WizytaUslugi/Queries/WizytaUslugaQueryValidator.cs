using FluentValidation;

namespace Application.WizytaUslugi.Queries
{
    public class WizytaUslugaQueryValidator : AbstractValidator<WizytaUslugaQuery>
    {
        public WizytaUslugaQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}