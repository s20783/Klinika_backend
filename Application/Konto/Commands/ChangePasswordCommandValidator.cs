using FluentValidation;

namespace Application.Konto.Commands
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.request.CurrentHaslo).MinimumLength(2).MaximumLength(30);

            RuleFor(x => x.request.NewHaslo).MaximumLength(30).WithMessage("Maksymalnie 30 znaków");
            RuleFor(x => x.request.NewHaslo).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$").WithMessage("Incorrect password");

            RuleFor(x => x.request.NewHaslo2).Equal(x => x.request.NewHaslo);
            RuleFor(x => x.request.NewHaslo2).MaximumLength(30).WithMessage("Maksymalnie 30 znaków");
            RuleFor(x => x.request.NewHaslo2).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{5,}$").WithMessage("Incorrect password");
        }
    }
}