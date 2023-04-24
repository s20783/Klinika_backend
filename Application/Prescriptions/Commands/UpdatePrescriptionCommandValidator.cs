using FluentValidation;

namespace Application.Recepty.Commands
{
    public class UpdatePrescriptionCommandValidator : AbstractValidator<UpdatePrescriptionCommand>
    {
        public UpdatePrescriptionCommandValidator()
        {
            RuleFor(x => x.ID_recepta).NotEmpty();

            RuleFor(x => x.Zalecenia).MinimumLength(2).MaximumLength(300);
        }
    }
}