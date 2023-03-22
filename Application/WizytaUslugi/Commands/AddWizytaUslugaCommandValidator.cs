using FluentValidation;

namespace Application.WizytaUslugi.Commands
{
    public class AddWizytaUslugaCommandValidator : AbstractValidator<AddWizytaUslugaCommand>
    {
        public AddWizytaUslugaCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}