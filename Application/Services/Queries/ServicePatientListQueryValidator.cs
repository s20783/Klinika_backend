using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class ServicePatientListQueryValidator : AbstractValidator<ServicePatientListQuery>
    {
        public ServicePatientListQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}