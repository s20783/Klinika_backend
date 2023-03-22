using FluentValidation;

namespace Application.Specjalizacje.Commands
{
    public class DeleteSpecjalizacjaCommandValidator : AbstractValidator<DeleteSpecjalizacjaCommand>
    {
        public DeleteSpecjalizacjaCommandValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();
        }
    }
}
