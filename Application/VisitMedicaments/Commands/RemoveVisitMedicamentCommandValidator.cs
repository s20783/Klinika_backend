using FluentValidation;

namespace Application.WizytaLeki.Commands
{
    internal class RemoveVisitMedicamentCommandValidator : AbstractValidator<RemoveVisitMedicamentCommand>
    {
        public RemoveVisitMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_lek).NotEmpty();

        }
    }
}