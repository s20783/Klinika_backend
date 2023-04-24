using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Commands
{
    public class AcceptVisitServicesCommandValidator : AbstractValidator<AcceptVisitServicesCommand>
    {
        public AcceptVisitServicesCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}