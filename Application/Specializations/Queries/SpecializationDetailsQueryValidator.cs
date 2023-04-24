using FluentValidation;

namespace Application.Specjalizacje.Queries
{
    public class SpecializationDetailsQueryValidator : AbstractValidator<SpecializationDetailsQuery>
    {
        public SpecializationDetailsQueryValidator()
        {
            RuleFor(x => x.ID_specjalizacja).NotEmpty();
        }
    }
}
