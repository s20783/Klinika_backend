using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klienci.Queries
{
    public class KlientQueryValidator : AbstractValidator<KlientQuery>
    {
        public KlientQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
