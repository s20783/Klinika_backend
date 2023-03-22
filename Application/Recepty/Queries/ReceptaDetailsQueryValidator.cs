using FluentValidation;

namespace Application.Recepty.Queries
{
    public class ReceptaDetailsQueryValidator : AbstractValidator<ReceptaDetailsQuery>
    {
        public ReceptaDetailsQueryValidator()
        {
            RuleFor(x => x.ID_recepta).NotEmpty();
        }
    }
}