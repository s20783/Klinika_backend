using FluentValidation;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class AddSpecjalizacjaWeterynarzCommandValidator : AbstractValidator<AddSpecjalizacjaWeterynarzCommand>
    {
        public AddSpecjalizacjaWeterynarzCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();

            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
