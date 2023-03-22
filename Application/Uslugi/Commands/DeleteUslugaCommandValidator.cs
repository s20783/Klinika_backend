using FluentValidation;

namespace Application.Uslugi.Commands
{
    public class DeleteUslugaCommandValidator : AbstractValidator<DeleteUslugaCommand>
    {
        public DeleteUslugaCommandValidator()
        {
            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}