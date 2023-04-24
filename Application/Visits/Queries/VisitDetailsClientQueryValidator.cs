using FluentValidation;

namespace Application.Wizyty.Queries
{
    public class VisitDetailsClientQueryValidator : AbstractValidator<VisitDetailsClientQuery>
    {
        public VisitDetailsClientQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}
