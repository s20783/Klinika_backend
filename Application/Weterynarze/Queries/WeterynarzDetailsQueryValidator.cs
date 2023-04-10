using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weterynarze.Queries
{
    public class WeterynarzDetailsQueryValidator : AbstractValidator<WeterynarzDetailsQuery>
    {
        public WeterynarzDetailsQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
