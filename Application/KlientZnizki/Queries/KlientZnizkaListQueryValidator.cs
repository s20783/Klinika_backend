using FluentValidation;

namespace Application.KlientZnizki.Queries
{
    public class KlientZnizkaListQueryValidator : AbstractValidator<KlientZnizkaListQuery>
    {
        public KlientZnizkaListQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}