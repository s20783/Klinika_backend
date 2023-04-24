using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Commands
{
    public class AddVisitDiseaseCommandValidator : AbstractValidator<AddVisitDiseaseCommand>
    {
        public AddVisitDiseaseCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_choroba).NotEmpty();
        }
    }
}