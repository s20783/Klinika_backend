using FluentValidation;

namespace Application.Wizyty.Commands
{
    public class UmowWizyteKlientCommandValidator : AbstractValidator<UmowWizyteKlientCommand>
    {
        public UmowWizyteKlientCommandValidator()
        {
            RuleFor(x => x.ID_Harmonogram).NotEmpty();

            RuleFor(x => x.ID_klient).NotEmpty();

            RuleFor(x => x.ID_pacjent).NotEmpty();

            RuleFor(x => x.Notatka).MaximumLength(300);
        }
    }
}
