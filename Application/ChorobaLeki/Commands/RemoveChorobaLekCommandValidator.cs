using FluentValidation;

namespace Application.ChorobaLeki.Commands
{
    public class RemoveChorobaLekCommandValidator : AbstractValidator<RemoveChorobaLekCommand>
    {
        public RemoveChorobaLekCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();

            RuleFor(x => x.ID_choroba).NotEmpty();
        }
    }
}