using FluentValidation;

namespace Application.Urlopy.Queries
{
    public class VacationVetQueryValidator : AbstractValidator<VacationVetQuery>
    {
        public VacationVetQueryValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}