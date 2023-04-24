using FluentValidation;

namespace Application.Szczepienia.Commands
{
    public class DeleteVaccinationCommandValidator : AbstractValidator<DeleteVaccinationCommand>
    {
        public DeleteVaccinationCommandValidator()
        {
            RuleFor(x => x.ID_szczepienie).NotEmpty();
        }
    }
}