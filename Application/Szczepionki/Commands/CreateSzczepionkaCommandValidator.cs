using FluentValidation;

namespace Application.Szczepionki.Commands
{
    public class CreateSzczepionkaCommandValidator : AbstractValidator<CreateSzczepionkaCommand>
    {
        public CreateSzczepionkaCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).Length(2,50);

            RuleFor(x => x.request.Producent).MaximumLength(50);

            RuleFor(x => x.request.Zastosowanie).MaximumLength(100);

            RuleFor(x => x.request.OkresWaznosci).GreaterThan(0);
        }
    }
}