using FluentValidation;

namespace Application.Uslugi.Commands
{
    public class DeleteServiceCommandValidator : AbstractValidator<DeleteServiceCommand>
    {
        public DeleteServiceCommandValidator()
        {
            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}