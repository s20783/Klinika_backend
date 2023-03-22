using FluentValidation;

namespace Application.Specjalizacje.Commands
{
    public class CreateSpecjalizacjaCommandValidator : AbstractValidator<CreateSpecjalizacjaCommand>
    {
        public CreateSpecjalizacjaCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Opis).MaximumLength(300);
        }
    }
}
