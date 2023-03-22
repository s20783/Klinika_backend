using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramKlientQueryValidator : AbstractValidator<HarmonogramKlientQuery>
    {
        public HarmonogramKlientQueryValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}