using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class CreateWizytaCommandValidator : AbstractValidator<CreateWizytaCommand>
    {
        public CreateWizytaCommandValidator()
        {
            RuleFor(x => x.ID_harmonogram).NotEmpty();

            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}