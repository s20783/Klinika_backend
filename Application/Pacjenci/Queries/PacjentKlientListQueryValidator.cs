using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PacjentKlientListQueryValidator : AbstractValidator<PacjentKlientListQuery>
    {
        public PacjentKlientListQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
