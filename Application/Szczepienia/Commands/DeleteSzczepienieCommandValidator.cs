using FluentValidation;

namespace Application.Szczepienia.Commands
{
    public class DeleteSzczepienieCommandValidator : AbstractValidator<DeleteSzczepienieCommand>
    {
        public DeleteSzczepienieCommandValidator()
        {
            RuleFor(x => x.ID_szczepienie).NotEmpty();
        }
    }
}