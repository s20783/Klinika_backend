using FluentValidation;

namespace Application.Pacjenci.Commands
{
    public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
    {
        public DeletePatientCommandValidator()
        {
            RuleFor(x => x.ID_Pacjent).NotEmpty();
        }
    }
}
