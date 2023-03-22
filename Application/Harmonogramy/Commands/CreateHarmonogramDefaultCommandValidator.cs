using FluentValidation;
using System;

namespace Application.Harmonogramy.Commands
{
    public class CreateHarmonogramDefaultCommandValidator : AbstractValidator<CreateHarmonogramDefaultCommand>
    {
        public CreateHarmonogramDefaultCommandValidator()
        {
            RuleFor(x => x.Data).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}