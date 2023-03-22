using FluentValidation;

namespace Application.ChorobaLeki.Queries
{
    public class ChorobaLekListQueryValidator : AbstractValidator<ChorobaLekListQuery>
    {
        public ChorobaLekListQueryValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();
        }
    }
}