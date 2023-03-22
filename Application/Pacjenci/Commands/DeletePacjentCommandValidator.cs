using FluentValidation;

namespace Application.Pacjenci.Commands
{
    public class DeletePacjentCommandValidator : AbstractValidator<DeletePacjentCommand>
    {
        public DeletePacjentCommandValidator()
        {
            RuleFor(x => x.ID_Pacjent).NotEmpty();
        }
    }
}
