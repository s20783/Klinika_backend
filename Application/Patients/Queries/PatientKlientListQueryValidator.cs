using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PatientKlientListQueryValidator : AbstractValidator<PatientKlientListQuery>
    {
        public PatientKlientListQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
