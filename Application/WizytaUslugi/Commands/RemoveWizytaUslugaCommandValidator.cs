using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Commands
{
    public class RemoveWizytaUslugaCommandValidator : AbstractValidator<RemoveWizytaUslugaCommand>
    {
        public RemoveWizytaUslugaCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}