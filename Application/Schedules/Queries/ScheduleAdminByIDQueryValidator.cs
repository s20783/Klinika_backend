using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class ScheduleAdminByIDQueryValidator : AbstractValidator<ScheduleAdminByIDQuery>
    {
        public ScheduleAdminByIDQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Date).NotEmpty();
        }
    }
}