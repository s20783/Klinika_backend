using FluentValidation;

namespace Application.Urlopy.Queries
{
    public class VacationDetailsQueryValidator : AbstractValidator<VacationDetailsQuery>
    {
        public VacationDetailsQueryValidator()
        {
            RuleFor(x => x.ID_urlop).NotEmpty();
        }
    }
}