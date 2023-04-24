using FluentValidation;

namespace Application.Szczepienia.Queries
{
    public class VaccinationPacjentQueryValidator : AbstractValidator<VaccinationPacjentQuery>
    {
        public VaccinationPacjentQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}