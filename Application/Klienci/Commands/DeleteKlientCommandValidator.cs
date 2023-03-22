using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klienci.Commands
{
    public class DeleteKlientCommandValidator : AbstractValidator<DeleteKlientCommand>
    {
        public DeleteKlientCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}
