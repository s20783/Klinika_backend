using FluentValidation;

namespace Application.GodzinaPracy.Commands
{
    public class DeleteGodzinyPracyCommandValidator : AbstractValidator<DeleteGodzinyPracyCommand>
    {
        public DeleteGodzinyPracyCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Day).NotEmpty();
        }
    }
}
