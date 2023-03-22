using FluentValidation;

namespace Application.Szczepienia.Queries
{
    public class SzczepieniePacjentQueryValidator : AbstractValidator<SzczepieniePacjentQuery>
    {
        public SzczepieniePacjentQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}