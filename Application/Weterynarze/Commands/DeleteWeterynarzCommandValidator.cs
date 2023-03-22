using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class DeleteWeterynarzCommandValidator : AbstractValidator<DeleteWeterynarzCommand>
    {
        public DeleteWeterynarzCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
