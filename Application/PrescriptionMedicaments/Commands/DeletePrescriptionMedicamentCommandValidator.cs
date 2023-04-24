using FluentValidation;

namespace Application.ReceptaLeki.Commands
{
    public class DeletePrescriptionMedicamentCommandValidator : AbstractValidator<DeletePrescriptionMedicamentCommand>
    {
        public DeletePrescriptionMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_Recepta).NotEmpty();

            RuleFor(x => x.ID_Lek).NotEmpty();
        }
    }
}