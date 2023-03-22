using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Commands
{
    public class RemoveWizytaChorobaCommandValidator : AbstractValidator<RemoveWizytaChorobaCommand>
    {
        public RemoveWizytaChorobaCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_choroba).NotEmpty();
        }
    }
}