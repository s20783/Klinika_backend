using FluentValidation;
using System;

namespace Application.LekiWMagazynie.Commands
{
    public class CreateStanLekuCommandValidator : AbstractValidator<CreateStanLekuCommand>
    {
        public CreateStanLekuCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();

            RuleFor(x => x.request.Ilosc).NotEmpty().GreaterThan(0).LessThanOrEqualTo(9999);

            RuleFor(x => x.request.DataWaznosci).NotEmpty().GreaterThan(DateTime.Now);
        }
    }
}