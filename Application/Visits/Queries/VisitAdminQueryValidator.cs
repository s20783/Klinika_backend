using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class VisitAdminQueryValidator : AbstractValidator<VisitAdminQuery>
    {
        public VisitAdminQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}