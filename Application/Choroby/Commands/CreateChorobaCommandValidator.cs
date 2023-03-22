using FluentValidation;

namespace Application.Choroby.Commands
{
    public class CreateChorobaCommandValidator : AbstractValidator<CreateChorobaCommand>
    {
        public CreateChorobaCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).MinimumLength(2).MaximumLength(100);

            RuleFor(x => x.request.Opis).MaximumLength(500);

            RuleFor(x => x.request.NazwaLacinska).MinimumLength(2).MaximumLength(50);
        }
    }
}