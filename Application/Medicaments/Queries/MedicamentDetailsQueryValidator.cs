using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class MedicamentDetailsQueryValidator : AbstractValidator<MedicamentDetailsQuery>
    {
        public MedicamentDetailsQueryValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();
        }
    }
}
