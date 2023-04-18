using FluentValidation;

namespace Application.Choroby.Commands
{
    public class UpdateChorobaCommandValidator : AbstractValidator<UpdateChorobaCommand>
    {
        public UpdateChorobaCommandValidator()
        {
            RuleFor(x => x.ID_Choroba).NotEmpty();

            RuleFor(x => x.request.Nazwa).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Opis).MaximumLength(500);

            RuleFor(x => x.request.NazwaLacinska).MaximumLength(50);
        }
    }
}