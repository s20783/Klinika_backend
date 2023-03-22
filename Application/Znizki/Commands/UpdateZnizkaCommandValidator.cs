using FluentValidation;

namespace Application.Znizki.Commands
{
    public class UpdateZnizkaCommandValidator : AbstractValidator<UpdateZnizkaCommand>
    {
        public UpdateZnizkaCommandValidator()
        {
            RuleFor(x => x.ID_znizka).NotEmpty();

            RuleFor(x => x.Nazwa).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.Procent).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}