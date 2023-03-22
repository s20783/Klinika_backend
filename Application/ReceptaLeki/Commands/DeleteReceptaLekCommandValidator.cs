using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ReceptaLeki.Commands
{
    public class DeleteReceptaLekCommandValidator : AbstractValidator<DeleteReceptaLekCommand>
    {
        public DeleteReceptaLekCommandValidator()
        {
            RuleFor(x => x.ID_Recepta).NotEmpty();

            RuleFor(x => x.ID_Lek).NotEmpty();
        }
    }
}