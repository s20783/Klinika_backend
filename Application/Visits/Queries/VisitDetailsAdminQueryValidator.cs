using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class VisitDetailsAdminQueryValidator : AbstractValidator<VisitDetailsAdminQuery>
    {
        public VisitDetailsAdminQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
