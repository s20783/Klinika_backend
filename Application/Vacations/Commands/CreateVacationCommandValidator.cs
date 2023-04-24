using FluentValidation;
using System;

namespace Application.Urlopy.Commands
{
    public class CreateVacationCommandValidator : AbstractValidator<CreateVacationCommand>
    {
        public CreateVacationCommandValidator()
        {
            RuleFor(x => x.request.ID_weterynarz).NotEmpty();

            RuleFor(x => x.request.Dzien).GreaterThanOrEqualTo(DateTime.Now.Date);
        }
    }
}