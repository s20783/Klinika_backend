using FluentValidation;

namespace Application.ChorobaLeki.Commands
{
    public class RemoveDiseaseMedicamentCommandValidator : AbstractValidator<RemoveDiseaseMedicamentCommand>
    {
        public RemoveDiseaseMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();

            RuleFor(x => x.ID_choroba).NotEmpty();
        }
    }
}