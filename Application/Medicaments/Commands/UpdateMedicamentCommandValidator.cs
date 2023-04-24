using FluentValidation;

namespace Application.Leki.Commands
{
    public class UpdateMedicamentCommandValidator : AbstractValidator<UpdateMedicamentCommand>
    {
        public UpdateMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();

            RuleFor(x => x.request.Nazwa).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.JednostkaMiary).MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Producent).MaximumLength(50);
        }
    }
}