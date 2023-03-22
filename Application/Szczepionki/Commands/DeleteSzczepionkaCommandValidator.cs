using FluentValidation;

namespace Application.Szczepionki.Commands
{
    public class DeleteSzczepionkaCommandValidator : AbstractValidator<DeleteSzczepionkaCommand>
    {
        public DeleteSzczepionkaCommandValidator()
        {
            RuleFor(x => x.ID_szczepionka).NotEmpty();
        }
    }
}