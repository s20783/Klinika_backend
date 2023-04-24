using FluentValidation;

namespace Application.WizytaChoroby.Queries
{
    public class VisitDiseaseQueryValidator : AbstractValidator<VisitDiseaseQuery>
    {
        public VisitDiseaseQueryValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
