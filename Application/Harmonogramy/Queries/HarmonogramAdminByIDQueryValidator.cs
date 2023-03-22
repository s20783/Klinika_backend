using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramAdminByIDQueryValidator : AbstractValidator<HarmonogramAdminByIDQuery>
    {
        public HarmonogramAdminByIDQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Date).NotEmpty();
        }
    }
}