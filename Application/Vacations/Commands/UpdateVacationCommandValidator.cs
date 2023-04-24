using FluentValidation;
using System;

namespace Application.Urlopy.Commands
{
    public class UpdateVacationCommandValidator : AbstractValidator<UpdateVacationCommand>
    {
        public UpdateVacationCommandValidator()
        {
            RuleFor(x => x.ID_urlop).NotEmpty();

            RuleFor(x => x.request.ID_weterynarz).NotEmpty();

            RuleFor(x => x.request.Dzien).GreaterThanOrEqualTo(DateTime.Now.Date);
        }
    }
}