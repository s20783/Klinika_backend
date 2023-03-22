using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class UpdateWizytaInfoCommandValidator : AbstractValidator<UpdateWizytaInfoCommand>
    {
        public UpdateWizytaInfoCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_weterynarz).NotEmpty();

            RuleFor(x => x.request.ID_Pacjent).NotEmpty();

            RuleFor(x => x.request.Opis).MinimumLength(2).MaximumLength(500);
        }
    }
}