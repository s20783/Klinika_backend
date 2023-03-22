using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Queries
{
    public class GodzinyPracyDzienQueryValidator : AbstractValidator<GodzinyPracyDzienQuery>
    {
        public GodzinyPracyDzienQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.Dzien).GreaterThanOrEqualTo(0).LessThanOrEqualTo(6);
        }
    }
}