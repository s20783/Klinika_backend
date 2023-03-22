using FluentValidation;

namespace Application.Uslugi.Commands
{
    public class UpdateUslugaCommandValidator : AbstractValidator<UpdateUslugaCommand>
    {
        public UpdateUslugaCommandValidator()
        {
            RuleFor(x => x.ID_usluga).NotEmpty();

            RuleFor(x => x.request.NazwaUslugi).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Opis).MaximumLength(300);

            RuleFor(x => x.request.Cena).GreaterThanOrEqualTo(0).LessThan(9999);

            RuleFor(x => x.request.Narkoza).NotNull();

            RuleFor(x => x.request.Dolegliwosc).MaximumLength(50);
        }
    }
}