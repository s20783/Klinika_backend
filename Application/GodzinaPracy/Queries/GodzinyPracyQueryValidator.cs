using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Queries
{
    public class GodzinyPracyQueryValidator : AbstractValidator<GodzinyPracyQuery>
    {
        public GodzinyPracyQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
