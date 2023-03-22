using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaAdminCommandValidator : AbstractValidator<DeleteWizytaAdminCommand>
    {
        public DeleteWizytaAdminCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
