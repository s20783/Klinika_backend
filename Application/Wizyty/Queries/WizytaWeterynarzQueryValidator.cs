using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class WizytaWeterynarzQueryValidator : AbstractValidator<WizytaWeterynarzQuery>
    {
        public WizytaWeterynarzQueryValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
