using FluentValidation;
using System;

namespace Application.GodzinaPracy.Commands
{
    public class CreateWorkingHoursCommandValidator : AbstractValidator<CreateWorkingHoursCommand>
    {
        public CreateWorkingHoursCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Request.GodzinaRozpoczecia).GreaterThanOrEqualTo(new TimeSpan(7, 0, 0));
        }
    }
}
