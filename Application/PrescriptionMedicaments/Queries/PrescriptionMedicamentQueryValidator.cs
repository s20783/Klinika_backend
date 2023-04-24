using FluentValidation;

namespace Application.ReceptaLeki.Queries
{
    public class PrescriptionMedicamentQueryValidator : AbstractValidator<PrescriptionMedicamentLekQuery>
    {
        public PrescriptionMedicamentQueryValidator()
        {
            RuleFor(x => x.ID_Recepta).NotEmpty();
        }
    }
}