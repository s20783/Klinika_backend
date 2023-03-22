using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class DeleteHarmonogramByIdCommandValidator : AbstractValidator<DeleteHarmonogramByIdCommand>
    {
        public DeleteHarmonogramByIdCommandValidator()
        {
            RuleFor(x => x.Data).GreaterThanOrEqualTo(DateTime.Now);

            RuleFor(x => x.ID_osoba).NotEmpty();
        }
    }
}