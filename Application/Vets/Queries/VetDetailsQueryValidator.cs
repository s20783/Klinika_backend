using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weterynarze.Queries
{
    public class VetDetailsQueryValidator : AbstractValidator<VetDetailsQuery>
    {
        public VetDetailsQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
