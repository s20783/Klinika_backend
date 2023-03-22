using FluentValidation;

namespace Application.Urlopy.Queries
{
    public class UrlopWeterynarzQueryValidator : AbstractValidator<UrlopWeterynarzQuery>
    {
        public UrlopWeterynarzQueryValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}