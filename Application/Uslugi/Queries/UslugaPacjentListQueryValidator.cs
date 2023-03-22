using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class UslugaPacjentListQueryValidator : AbstractValidator<UslugaPacjentListQuery>
    {
        public UslugaPacjentListQueryValidator()
        {
            RuleFor(x => x.ID_pacjent).NotEmpty();
        }
    }
}