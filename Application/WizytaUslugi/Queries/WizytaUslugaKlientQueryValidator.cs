using FluentValidation;

namespace Application.WizytaUslugi.Queries
{
    public class WizytaUslugaKlientQueryValidator : AbstractValidator<WizytaUslugaKlientQuery>
    {
        public WizytaUslugaKlientQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}