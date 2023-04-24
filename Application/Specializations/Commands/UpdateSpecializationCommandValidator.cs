using FluentValidation;

namespace Application.Specjalizacje.Commands
{
    public class UpdateSpecializationCommandValidator : AbstractValidator<UpdateSpecializationCommand>
    {
        public UpdateSpecializationCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();

            RuleFor(x => x.request.Nazwa).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Opis).MaximumLength(300);
        }
    }
}
