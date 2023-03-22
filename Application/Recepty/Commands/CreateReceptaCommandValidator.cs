using FluentValidation;

namespace Application.Recepty.Commands
{
    public class CreateReceptaCommandValidator : AbstractValidator<CreateReceptaCommand>
    {
        public CreateReceptaCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.Zalecenia).MinimumLength(2).MaximumLength(300);
        }
    }
}