using FluentValidation;

namespace Application.Choroby.Commands
{
    public class CreateDiseaseCommandValidator : AbstractValidator<CreateDiseaseCommand>
    {
        public CreateDiseaseCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).MinimumLength(2).MaximumLength(100);

            RuleFor(x => x.request.Opis).MaximumLength(500);

            RuleFor(x => x.request.NazwaLacinska).MaximumLength(50);
        }
    }
}