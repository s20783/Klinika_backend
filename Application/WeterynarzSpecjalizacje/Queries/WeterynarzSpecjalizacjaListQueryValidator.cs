using FluentValidation;

namespace Application.WeterynarzSpecjalizacje.Queries
{
    public class WeterynarzSpecjalizacjaListQueryValidator : AbstractValidator<WeterynarzSpecjalizacjaListQuery>
    {
        public WeterynarzSpecjalizacjaListQueryValidator()
        {
            RuleFor(x => x.ID_weterynarz).NotEmpty();
        }
    }
}
