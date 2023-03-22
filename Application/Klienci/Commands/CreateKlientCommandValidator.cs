using FluentValidation;

namespace Application.Klienci.Commands
{
    public class CreateKlientCommandValidator : AbstractValidator<CreateKlientCommand>
    {
        public CreateKlientCommandValidator()
        {
            RuleFor(x => x.request.Imie).MinimumLength(2).WithMessage("To pole wymaga od 2 do 50 znaków");
            RuleFor(x => x.request.Imie).MaximumLength(50).WithMessage("To pole wymaga od 2 do 50 znaków");

            RuleFor(x => x.request.Nazwisko).MinimumLength(2).WithMessage("To pole wymaga od 2 do 50 znaków");
            RuleFor(x => x.request.Nazwisko).MaximumLength(50).WithMessage("To pole wymaga od 2 do 50 znaków");

            RuleFor(x => x.request.Email).NotEmpty().MaximumLength(50).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)").WithMessage("Niepoprawny format");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12).WithMessage("Niepoprawny format");

            RuleFor(x => x.request.NazwaUzytkownika).MinimumLength(2).WithMessage("To pole wymaga od 2 do 50 znaków");
            RuleFor(x => x.request.NazwaUzytkownika).MaximumLength(50).WithMessage("To pole wymaga od 2 do 50 znaków");

            RuleFor(x => x.request.Haslo).MaximumLength(30).WithMessage("Maksymalnie 30 znaków");
            RuleFor(x => x.request.Haslo).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$").WithMessage("Hasło musi zawierać wielką literę i cyfrę");

            RuleFor(x => x.request.Haslo2).MaximumLength(30).WithMessage("Maksymalnie 30 znaków");
            RuleFor(x => x.request.Haslo2).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$").WithMessage("Hasło musi zawierać wielką literę i cyfrę");
        }
    }
}