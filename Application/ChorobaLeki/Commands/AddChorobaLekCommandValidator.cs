using FluentValidation;

namespace Application.ChorobaLeki.Commands
{
    public class AddChorobaLekCommandValidator : AbstractValidator<AddChorobaLekCommand>
    {
        public AddChorobaLekCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();

            RuleFor(x => x.ID_choroba).NotEmpty();
        }
    }
}