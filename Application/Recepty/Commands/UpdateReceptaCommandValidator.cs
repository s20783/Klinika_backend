using FluentValidation;

namespace Application.Recepty.Commands
{
    public class UpdateReceptaCommandValidator : AbstractValidator<UpdateReceptaCommand>
    {
        public UpdateReceptaCommandValidator()
        {
            RuleFor(x => x.ID_recepta).NotEmpty();

            RuleFor(x => x.Zalecenia).MinimumLength(2).MaximumLength(300);
        }
    }
}