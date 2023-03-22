using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Commands
{
    public class AcceptWizytaUslugaCommandValidator : AbstractValidator<AcceptWizytaUslugaCommand>
    {
        public AcceptWizytaUslugaCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}