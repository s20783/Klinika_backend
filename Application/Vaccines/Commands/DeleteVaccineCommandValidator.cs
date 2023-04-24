using FluentValidation;

namespace Application.Szczepionki.Commands
{
    public class DeleteVaccineCommandValidator : AbstractValidator<DeleteVaccineCommand>
    {
        public DeleteVaccineCommandValidator()
        {
            RuleFor(x => x.ID_szczepionka).NotEmpty();
        }
    }
}