using FluentValidation;

namespace Application.Choroby.Commands
{
    public class DeleteDiseaseCommandValidator : AbstractValidator<DeleteDiseaseCommand>
    {
        public DeleteDiseaseCommandValidator()
        {
            RuleFor(x => x.ID_Choroba).NotEmpty();
        }
    }
}