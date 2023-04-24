using FluentValidation;

namespace Application.Szczepienia.Queries
{
    public class VaccinationDetailsQueryValidator : AbstractValidator<VaccinationDetailsQuery>
    {
        public VaccinationDetailsQueryValidator()
        {
            RuleFor(x => x.ID_szczepienie).NotEmpty();
        }
    }
}