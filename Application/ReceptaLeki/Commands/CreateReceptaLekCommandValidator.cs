using FluentValidation;

namespace Application.ReceptaLeki.Commands
{
    public class CreateReceptaLekCommandValidator : AbstractValidator<CreateReceptaLekCommand>
    {
        public CreateReceptaLekCommandValidator()
        {
            RuleFor(x => x.ID_Recepta).NotEmpty();

            RuleFor(x => x.ID_Lek).NotEmpty();

            RuleFor(x => x.Ilosc).GreaterThan(0).LessThan(99);
        }
    }
}