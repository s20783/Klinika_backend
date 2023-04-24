using FluentValidation;
using System;

namespace Application.GodzinaPracy.Commands
{
    public class UpdateWorkingHoursCommandValidator : AbstractValidator<UpdateWorkingHoursCommand>
    {
        public UpdateWorkingHoursCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Request.GodzinaRozpoczecia).GreaterThanOrEqualTo(new TimeSpan(7, 0, 0));
        }
    }
}
