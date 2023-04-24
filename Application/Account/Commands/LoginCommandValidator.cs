using FluentValidation;

namespace Application.Konto.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.request.NazwaUzytkownika).MinimumLength(2).WithMessage("To pole wymaga od 2 do 30 znaków");
            RuleFor(x => x.request.NazwaUzytkownika).MaximumLength(30).WithMessage("To pole wymaga od 2 do 30 znaków");

            RuleFor(x => x.request.Haslo).MinimumLength(2).WithMessage("To pole wymaga od 2 do 30 znaków");
            RuleFor(x => x.request.Haslo).MaximumLength(30).WithMessage("To pole wymaga od 2 do 30 znaków");
        }
    }
}
