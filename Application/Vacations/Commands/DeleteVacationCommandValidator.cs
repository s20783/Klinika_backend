using FluentValidation;

namespace Application.Urlopy.Commands
{
    public class DeleteVacationCommandValidator : AbstractValidator<DeleteVacationCommand>
    {
        public DeleteVacationCommandValidator()
        {
            RuleFor(x => x.ID_urlop).NotEmpty();
        }
    }
}