using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class UpdateVisitDateCommandValidator : AbstractValidator<UpdateVisitDateCommand>
    {
        public UpdateVisitDateCommandValidator()
        {
            RuleFor(x => x.ID_harmonogram).NotEmpty();

            //RuleFor(x => x.ID_klient).NotEmpty();

            RuleFor(x => x.ID_pacjent).NotEmpty();

            RuleFor(x => x.ID_wizyta).NotEmpty();

            RuleFor(x => x.Notatka).MaximumLength(300);
        }
    }
}