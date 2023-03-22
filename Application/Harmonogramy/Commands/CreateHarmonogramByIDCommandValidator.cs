using FluentValidation;
using System;

namespace Application.Harmonogramy.Commands
{
    public class CreateHarmonogramByIDCommandValidator : AbstractValidator<CreateHarmonogramByIDCommand>
    {
        public CreateHarmonogramByIDCommandValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();

            RuleFor(x => x.Data).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}