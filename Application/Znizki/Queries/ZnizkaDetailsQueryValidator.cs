using FluentValidation;

namespace Application.Znizki.Queries
{
    public class ZnizkaDetailsQueryValidator : AbstractValidator<ZnizkaDetailsQuery>
    {
        public ZnizkaDetailsQueryValidator()
        {
            RuleFor(x => x.ID_znizka).NotEmpty();
        }
    }
}