using FluentValidation;

namespace Application.Choroby.Queries
{
    public class ChorobaDetailsQueryValidator : AbstractValidator<ChorobaDetailsQuery>
    {
        public ChorobaDetailsQueryValidator()
        {
            RuleFor(x => x.ID_Choroba).NotEmpty();
        }
    }
}