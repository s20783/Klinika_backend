using FluentValidation;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class RemoveVetSpecializationCommandValidator : AbstractValidator<RemoveVetSpecializationCommand>
    {
        public RemoveVetSpecializationCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();

            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
