using FluentValidation;
using System;

namespace Application.Weterynarze.Commands
{
    public class CreateVetCommandValidator : AbstractValidator<CreateVetCommand>
    {
        public CreateVetCommandValidator()
        {
            RuleFor(x => x.request.Imie).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Nazwisko).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Email).MinimumLength(6).MaximumLength(50).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})");

            RuleFor(x => x.request.DataUrodzenia).NotEmpty().LessThan(DateTime.Now);

            RuleFor(x => x.request.DataZatrudnienia).NotEmpty();

            RuleFor(x => x.request.Pensja).NotEmpty().GreaterThan(0).LessThanOrEqualTo(99999);
        }
    }
}