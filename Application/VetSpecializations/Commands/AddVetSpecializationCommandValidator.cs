using FluentValidation;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class AddVetSpecializationCommandValidator : AbstractValidator<AddVetSpecializationCommand>
    {
        public AddVetSpecializationCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();

            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
