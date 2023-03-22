using FluentValidation;

namespace Application.Specjalizacje.Queries
{
    public class SpecjalizacjaDetailsQueryValidator : AbstractValidator<SpecjalizacjaDetailsQuery>
    {
        public SpecjalizacjaDetailsQueryValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();
        }
    }
}
