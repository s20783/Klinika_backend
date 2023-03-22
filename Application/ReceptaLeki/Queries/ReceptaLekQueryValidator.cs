using FluentValidation;

namespace Application.ReceptaLeki.Queries
{
    public class ReceptaLekQueryValidator : AbstractValidator<ReceptaLekQuery>
    {
        public ReceptaLekQueryValidator()
        {
            RuleFor(x => x.ID_Recepta).NotEmpty();
        }
    }
}