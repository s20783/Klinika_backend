using FluentValidation;

namespace Application.Weterynarze.Commands
{
    public class UpdateWeterynarzCommandValidator : AbstractValidator<UpdateWeterynarzCommand>
    {
        public UpdateWeterynarzCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.request.Imie).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Nazwisko).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.DataUrodzenia).NotEmpty();

            RuleFor(x => x.request.DataZatrudnienia).NotEmpty();

            RuleFor(x => x.request.Pensja).NotEmpty().GreaterThan(0).LessThanOrEqualTo(99999);

            RuleFor(x => x.request.Email).MinimumLength(6).MaximumLength(50).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)");

            RuleFor(x => x.request.NumerTelefonu).Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12);
        }
    }
}