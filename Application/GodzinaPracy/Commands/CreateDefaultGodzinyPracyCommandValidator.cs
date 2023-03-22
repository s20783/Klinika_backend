using FluentValidation;

namespace Application.GodzinaPracy.Commands
{
    public class CreateDefaultGodzinyPracyCommandValidator : AbstractValidator<CreateDefaultGodzinyPracyCommand>
    {
        public CreateDefaultGodzinyPracyCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
