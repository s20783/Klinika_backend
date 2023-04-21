using FluentValidation;
using System;

namespace Application.GodzinaPracy.Commands
{
    public class CreateGodzinyPracyCommandValidator : AbstractValidator<CreateGodzinyPracyCommand>
    {
        public CreateGodzinyPracyCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Request.GodzinaRozpoczecia).GreaterThanOrEqualTo(new TimeSpan(7, 0, 0));
        }
    }
}
