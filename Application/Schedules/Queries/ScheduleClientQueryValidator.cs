using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class ScheduleClientQueryValidator : AbstractValidator<ScheduleClientQuery>
    {
        public ScheduleClientQueryValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}