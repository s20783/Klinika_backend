using FluentValidation;

namespace Application.Choroby.Commands
{
    public class DeleteChorobaCommandValidator : AbstractValidator<DeleteChorobaCommand>
    {
        public DeleteChorobaCommandValidator()
        {
            RuleFor(x => x.ID_Choroba).NotEmpty();
        }
    }
}