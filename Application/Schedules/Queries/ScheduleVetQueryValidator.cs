using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class ScheduleVetQueryValidator : AbstractValidator<ScheduleVetQuery>
    {
        public ScheduleVetQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Date).NotEmpty();
        }
    }
}