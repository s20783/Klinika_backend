using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class DeleteVisitClientCommandValidator : AbstractValidator<DeleteVisitClientCommand>
    {
        public DeleteVisitClientCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
