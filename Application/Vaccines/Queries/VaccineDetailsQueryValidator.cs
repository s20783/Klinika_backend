using FluentValidation;

namespace Application.Szczepionki.Queries
{
    public class VaccineDetailsQueryValidator : AbstractValidator<VaccineDetailsQuery>
    {
        public VaccineDetailsQueryValidator()
        {
            RuleFor(x => x.ID_szczepionka).NotEmpty();
        }
    }
}