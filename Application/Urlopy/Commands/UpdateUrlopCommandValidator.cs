using FluentValidation;
using System;

namespace Application.Urlopy.Commands
{
    public class UpdateUrlopCommandValidator : AbstractValidator<UpdateUrlopCommand>
    {
        public UpdateUrlopCommandValidator()
        {
            RuleFor(x => x.ID_urlop).NotEmpty();

            RuleFor(x => x.request.ID_weterynarz).NotEmpty();

            RuleFor(x => x.request.Dzien).GreaterThanOrEqualTo(DateTime.Now.Date);
        }
    }
}