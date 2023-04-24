using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(x => x.request.Agresywne).NotNull();

            RuleFor(x => x.request.Ubezplodnienie).NotNull();

            RuleFor(x => x.request.DataUrodzenia).NotEmpty().LessThanOrEqualTo(DateTime.Now);

            RuleFor(x => x.request.Gatunek).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.IdOsoba).NotEmpty();

            RuleFor(x => x.request.Masc).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Plec).NotEmpty().Matches(@"M|F");

            RuleFor(x => x.request.Rasa).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Nazwa).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Waga).NotEmpty().GreaterThan(0).LessThanOrEqualTo(999);
        }
    }
}
