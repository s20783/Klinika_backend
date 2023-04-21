using FluentValidation;
using System;

namespace Application.GodzinaPracy.Commands
{
    public class UpdateGodzinyPracyCommandValidator : AbstractValidator<UpdateGodzinyPracyCommand>
    {
        public UpdateGodzinyPracyCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Request.GodzinaRozpoczecia).GreaterThanOrEqualTo(new TimeSpan(7, 0, 0));
        }
    }
}
