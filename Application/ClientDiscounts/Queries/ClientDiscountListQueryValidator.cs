using FluentValidation;

namespace Application.KlientZnizki.Queries
{
    public class ClientDiscountListQueryValidator : AbstractValidator<ClientDiscountListQuery>
    {
        public ClientDiscountListQueryValidator()
        {
            RuleFor(x => x.ID_klient).NotEmpty();
        }
    }
}