using FluentValidation;

namespace Application.Szczepienia.Queries
{
    public class SzczepienieDetailsQueryValidator : AbstractValidator<SzczepienieDetailsQuery>
    {
        public SzczepienieDetailsQueryValidator()
        {
            RuleFor(x => x.ID_szczepienie).NotEmpty();
        }
    }
}