using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class VisitVetQueryValidator : AbstractValidator<VisitVetQuery>
    {
        public VisitVetQueryValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
