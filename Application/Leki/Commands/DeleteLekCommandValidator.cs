using FluentValidation;

namespace Application.Leki.Commands
{
    public class DeleteLekCommandValidator : AbstractValidator<DeleteLekCommand>
    {
        public DeleteLekCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();
        }
    }
}