using FluentValidation;

namespace Application.ChorobaLeki.Queries
{
    public class DiseaseMedicamentListQueryValidator : AbstractValidator<DiseaseMedicamentListQuery>
    {
        public DiseaseMedicamentListQueryValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();
        }
    }
}