using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaLeki.Queries
{
    public class WizytaLekListQueryValidator : AbstractValidator<WizytaLekListQuery>
    {
        public WizytaLekListQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}