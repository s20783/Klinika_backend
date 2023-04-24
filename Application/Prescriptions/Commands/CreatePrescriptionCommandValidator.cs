using FluentValidation;

namespace Application.Recepty.Commands
{
    public class CreatePrescriptionCommandValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.Zalecenia).MinimumLength(2).MaximumLength(300);
        }
    }
}