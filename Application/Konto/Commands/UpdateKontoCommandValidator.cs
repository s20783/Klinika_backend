using FluentValidation;

namespace Application.Konto.Commands
{
    public class UpdateKontoCommandValidator : AbstractValidator<UpdateKontoCommand>
    {
        public UpdateKontoCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.request.Email).NotEmpty().Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12);

            RuleFor(x => x.request.Haslo).MinimumLength(2).MaximumLength(30);

            RuleFor(x => x.request.NazwaUzytkownika).MinimumLength(2).MaximumLength(50);
        }
    }
}