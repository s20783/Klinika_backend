using FluentValidation;

namespace Application.Recepty.Queries
{
    public class PrescriptionPatientQueryValidator : AbstractValidator<PrescriptionPatientQuery>
    {
        public PrescriptionPatientQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}