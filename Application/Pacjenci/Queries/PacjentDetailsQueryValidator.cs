using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PacjentDetailsQueryValidator : AbstractValidator<PacjentDetailsQuery>
    {
        public PacjentDetailsQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}
