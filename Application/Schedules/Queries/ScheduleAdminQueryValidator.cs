using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class ScheduleAdminQueryValidator : AbstractValidator<ScheduleAdminQuery>
    {
        public ScheduleAdminQueryValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}