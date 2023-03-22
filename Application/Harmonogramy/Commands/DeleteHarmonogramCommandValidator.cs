using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class DeleteHarmonogramCommandValidator : AbstractValidator<DeleteHarmonogramCommand>
    {
        public DeleteHarmonogramCommandValidator()
        {
            RuleFor(x => x.Data).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}
