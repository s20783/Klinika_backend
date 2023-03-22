using FluentValidation;

namespace Application.LekiWMagazynie.Queries
{
    public class StanLekuQueryValidator : AbstractValidator<StanLekuQuery>
    {
        public StanLekuQueryValidator()
        {
            RuleFor(x => x.ID_stan_leku).NotEmpty();
        }
    }
}
