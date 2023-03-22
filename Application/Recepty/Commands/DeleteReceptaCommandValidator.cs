using FluentValidation;

namespace Application.Recepty.Commands
{
    public class DeleteReceptaCommandValidator : AbstractValidator<DeleteReceptaCommand>
    {
        public DeleteReceptaCommandValidator()
        {
            RuleFor(x => x.ID_recepta).NotEmpty();
        }
    }
}