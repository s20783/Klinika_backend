using FluentValidation;

namespace Application.LekiWMagazynie.Queries
{
    public class MedicamentWarehouseQueryValidator : AbstractValidator<MedicamentWarehouseQuery>
    {
        public MedicamentWarehouseQueryValidator()
        {
            RuleFor(x => x.ID_stan_leku).NotEmpty();
        }
    }
}
