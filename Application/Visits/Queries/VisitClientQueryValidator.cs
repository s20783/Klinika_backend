using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class VisitClientQueryValidator : AbstractValidator<VisitClientQuery>
    {
        public VisitClientQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}