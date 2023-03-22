using FluentValidation;

namespace Application.Leki.Commands
{
    public class CreateLekCommandValidator : AbstractValidator<CreateLekCommand>
    {
        public CreateLekCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.JednostkaMiary).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Producent).MaximumLength(50);
        }
    }
}