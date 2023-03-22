using FluentValidation;

namespace Application.Urlopy.Queries
{
    public class UrlopDetailsQueryValidator : AbstractValidator<UrlopDetailsQuery>
    {
        public UrlopDetailsQueryValidator()
        {
            RuleFor(x => x.ID_urlop).NotEmpty();
        }
    }
}