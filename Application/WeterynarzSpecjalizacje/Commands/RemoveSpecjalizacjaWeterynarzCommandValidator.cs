using FluentValidation;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class RemoveSpecjalizacjaWeterynarzCommandValidator : AbstractValidator<RemoveSpecjalizacjaWeterynarzCommand>
    {
        public RemoveSpecjalizacjaWeterynarzCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();

            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
