using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaKlientCommandValidator : AbstractValidator<DeleteWizytaKlientCommand>
    {
        public DeleteWizytaKlientCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
