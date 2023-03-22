using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaLeki.Commands
{
    internal class RemoveWizytaLekCommandValidator : AbstractValidator<RemoveWizytaLekCommand>
    {
        public RemoveWizytaLekCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_lek).NotEmpty();

        }
    }
}