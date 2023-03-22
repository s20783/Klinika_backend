using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Konto.Queries
{
    public class KontoQueryValidator : AbstractValidator<KontoQuery>
    {
        public KontoQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
