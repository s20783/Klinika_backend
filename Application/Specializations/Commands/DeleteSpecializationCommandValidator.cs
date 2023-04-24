using FluentValidation;

namespace Application.Specjalizacje.Commands
{
    public class DeleteSpecializationCommandValidator : AbstractValidator<DeleteSpecializationCommand>
    {
        public DeleteSpecializationCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();
        }
    }
}
