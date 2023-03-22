using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Commands
{
    public class DeleteGodzinyPracyCommandValidator : AbstractValidator<DeleteGodzinyPracyCommand>
    {
        public DeleteGodzinyPracyCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.dzien).NotEmpty();
        }
    }
}
