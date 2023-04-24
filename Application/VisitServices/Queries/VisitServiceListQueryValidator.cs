using FluentValidation;

namespace Application.WizytaUslugi.Queries
{
    public class VisitServiceListQueryValidator : AbstractValidator<VisitServiceListQuery>
    {
        public VisitServiceListQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}