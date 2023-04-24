using FluentValidation;

namespace Application.Recepty.Queries
{
    public class PrescriptionDetailsQueryValidator : AbstractValidator<PrescriptionDetailsQuery>
    {
        public PrescriptionDetailsQueryValidator()
        {
            RuleFor(x => x.ID_recepta).NotEmpty();
        }
    }
}