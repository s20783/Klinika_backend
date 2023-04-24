using FluentValidation;

namespace Application.ChorobaLeki.Commands
{
    public class AddDiseaseMedicamentCommandValidator : AbstractValidator<AddDiseaseMedicamentCommand>
    {
        public AddDiseaseMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();

            RuleFor(x => x.ID_choroba).NotEmpty();
        }
    }
}