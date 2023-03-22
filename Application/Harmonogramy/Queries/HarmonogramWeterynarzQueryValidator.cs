using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramWeterynarzQueryValidator : AbstractValidator<HarmonogramWeterynarzQuery>
    {
        public HarmonogramWeterynarzQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Date).NotEmpty();
        }
    }
}