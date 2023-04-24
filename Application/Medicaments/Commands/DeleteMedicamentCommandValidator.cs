using FluentValidation;

namespace Application.Leki.Commands
{
    public class DeleteMedicamentCommandValidator : AbstractValidator<DeleteMedicamentCommand>
    {
        public DeleteMedicamentCommandValidator()
        {
            RuleFor(x => x.ID_lek).NotEmpty();
        }
    }
}