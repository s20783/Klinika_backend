using FluentValidation;

namespace Application.Znizki.Queries
{
    public class DiscountDetailsQueryValidator : AbstractValidator<DiscountDetailsQuery>
    {
        public DiscountDetailsQueryValidator()
        {
            RuleFor(x => x.ID_znizka).NotEmpty();
        }
    }
}