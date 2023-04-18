using FluentValidation;

namespace Application.Szczepionki.Commands
{
    public class UpdateSzczepionkaCommandValidator : AbstractValidator<UpdateSzczepionkaCommand>
    {
        public UpdateSzczepionkaCommandValidator()
        {
            RuleFor(x => x.ID_szczepionka).NotEmpty();

            RuleFor(x => x.request.Nazwa).Length(2, 50);

            RuleFor(x => x.request.Producent).MaximumLength(50);

            RuleFor(x => x.request.Zastosowanie).MaximumLength(100);

            RuleFor(x => x.request.OkresWaznosci).GreaterThan(0);
        }
    }
}