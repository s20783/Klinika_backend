using FluentValidation;

namespace Application.Szczepionki.Queries
{
    public class SzczepionkaDetailsQueryValidator : AbstractValidator<SzczepionkaDetailsQuery>
    {
        public SzczepionkaDetailsQueryValidator()
        {
            RuleFor(x => x.ID_szczepionka).NotEmpty();
        }
    }
}