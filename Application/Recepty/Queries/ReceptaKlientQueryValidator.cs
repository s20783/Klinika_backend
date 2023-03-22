using FluentValidation;

namespace Application.Recepty.Queries
{
    public class ReceptaKlientQueryValidator : AbstractValidator<ReceptaKlientQuery>
    {
        public ReceptaKlientQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}