using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Queries
{
    public class WizytaUslugaKlinikaQueryValidator : AbstractValidator<WizytaUslugaKlinikaQuery>
    {
        public WizytaUslugaKlinikaQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}