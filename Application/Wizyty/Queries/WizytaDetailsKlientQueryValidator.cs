using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class WizytaDetailsKlientQueryValidator : AbstractValidator<WizytaDetailsKlientQuery>
    {
        public WizytaDetailsKlientQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}
