using FluentValidation;

namespace Application.Uslugi.Queries
{
    public class UslugaDetailsQueryValidator : AbstractValidator<UslugaDetailsQuery>
    {
        public UslugaDetailsQueryValidator()
        {
            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}