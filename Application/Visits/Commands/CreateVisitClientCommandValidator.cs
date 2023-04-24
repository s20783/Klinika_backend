using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class CreateVisitClientCommandValidator : AbstractValidator<CreateVisitClientCommand>
    {
        public CreateVisitClientCommandValidator()
        {
            RuleFor(x => x.ID_Harmonogram).NotEmpty();

            RuleFor(x => x.ID_klient).NotEmpty();

            RuleFor(x => x.ID_pacjent).NotEmpty();

            RuleFor(x => x.Notatka).MaximumLength(300);
        }
    }
}
