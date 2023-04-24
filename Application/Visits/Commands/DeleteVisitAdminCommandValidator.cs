using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class DeleteVisitAdminCommandValidator : AbstractValidator<DeleteVisitAdminCommand>
    {
        public DeleteVisitAdminCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
