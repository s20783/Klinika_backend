using FluentValidation;

namespace Application.Recepty.Queries
{
    public class ReceptaPacjentQueryValidator : AbstractValidator<ReceptaPacjentQuery>
    {
        public ReceptaPacjentQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}