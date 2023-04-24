using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class DeleteVetCommandValidator : AbstractValidator<DeleteVetCommand>
    {
        public DeleteVetCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
