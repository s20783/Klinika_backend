using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Queries
{
    public class WizytaChorobaQueryValidator : AbstractValidator<WizytaChorobaQuery>
    {
        public WizytaChorobaQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
