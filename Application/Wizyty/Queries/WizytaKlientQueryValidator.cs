using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class WizytaKlientQueryValidator : AbstractValidator<WizytaKlientQuery>
    {
        public WizytaKlientQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}