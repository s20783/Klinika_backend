using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaLeki.Commands
{
    public class AddWizytaLekCommandValidator : AbstractValidator<AddWizytaLekCommand>
    {
        public AddWizytaLekCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_Lek).NotEmpty();

            RuleFor(x => x.Ilosc).GreaterThan(0).LessThanOrEqualTo(999);
        }
    }
}