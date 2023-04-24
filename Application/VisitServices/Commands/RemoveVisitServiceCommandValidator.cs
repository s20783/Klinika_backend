using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Commands
{
    public class RemoveVisitServiceCommandValidator : AbstractValidator<RemoveVisitServiceCommand>
    {
        public RemoveVisitServiceCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}