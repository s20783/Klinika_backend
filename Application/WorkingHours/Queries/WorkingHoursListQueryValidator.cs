using FluentValidation;

namespace Application.GodzinaPracy.Queries
{
    public class WorkingHoursListQueryValidator : AbstractValidator<WorkingHoursListQuery>
    {
        public WorkingHoursListQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
