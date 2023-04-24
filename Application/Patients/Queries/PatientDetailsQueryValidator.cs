using FluentValidation;

namespace Application.Pacjenci.Queries
{
    public class PatientDetailsQueryValidator : AbstractValidator<PatientDetailsQuery>
    {
        public PatientDetailsQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}
