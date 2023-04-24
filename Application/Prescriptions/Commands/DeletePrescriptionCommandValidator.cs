using FluentValidation;

namespace Application.Recepty.Commands
{
    public class DeletePrescriptionCommandValidator : AbstractValidator<DeletePrescriptionCommand>
    {
        public DeletePrescriptionCommandValidator()
        {
            RuleFor(x => x.ID_recepta).NotEmpty();
        }
    }
}