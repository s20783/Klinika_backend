using FluentValidation;

namespace Application.WizytaLeki.Commands
{
    public class AddVisitMedicamentCommandValidator : AbstractValidator<AddVisitMedicamentCommand>
    {
        public AddVisitMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_Lek).NotEmpty();

            RuleFor(x => x.Ilosc).GreaterThan(0).LessThanOrEqualTo(999);
        }
    }
}