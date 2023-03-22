using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klienci.Commands
{
    public class CreateKlientKlinikaCommandValidator : AbstractValidator<CreateKlientKlinikaCommand>
    {
        public CreateKlientKlinikaCommandValidator()
        {
            RuleFor(x => x.request.Imie).MinimumLength(2).WithMessage("To pole wymaga od 2 do 50 znaków");
            RuleFor(x => x.request.Imie).MaximumLength(50).WithMessage("To pole wymaga od 2 do 50 znaków");

            RuleFor(x => x.request.Nazwisko).MinimumLength(2).WithMessage("To pole wymaga od 2 do 50 znaków");
            RuleFor(x => x.request.Nazwisko).MaximumLength(50).WithMessage("To pole wymaga od 2 do 50 znaków");

            RuleFor(x => x.request.Email).NotEmpty().MaximumLength(50).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)").WithMessage("Niepoprawny format");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12).WithMessage("Niepoprawny format");
        }
    }
}