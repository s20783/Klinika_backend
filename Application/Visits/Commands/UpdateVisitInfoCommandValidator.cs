using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class UpdateVisitInfoCommandValidator : AbstractValidator<UpdateVisitInfoCommand>
    {
        public UpdateVisitInfoCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.ID_weterynarz).NotEmpty();

            RuleFor(x => x.request.ID_Pacjent).NotEmpty();

            RuleFor(x => x.request.Opis).MinimumLength(2).MaximumLength(500);
        }
    }
}