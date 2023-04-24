using FluentValidation;

namespace Application.WizytaUslugi.Commands
{
    public class AddVisitServiceCommandValidator : AbstractValidator<AddVisitServiceCommand>
    {
        public AddVisitServiceCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_usluga).NotEmpty();
        }
    }
}