using FluentValidation;

namespace Application.Recepty.Queries
{
    public class PrescriptionClientQueryValidator : AbstractValidator<PrescriptionClientQuery>
    {
        public PrescriptionClientQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}