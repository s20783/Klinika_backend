using FluentValidation;

namespace Application.Urlopy.Commands
{
    public class DeleteUrlopCommandValidator : AbstractValidator<DeleteUrlopCommand>
    {
        public DeleteUrlopCommandValidator()
        {
            RuleFor(x => x.ID_urlop).NotEmpty();
        }
    }
}