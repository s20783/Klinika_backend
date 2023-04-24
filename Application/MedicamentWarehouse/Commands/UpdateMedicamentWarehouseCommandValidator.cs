using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Commands
{
    public class UpdateMedicamentWarehouseCommandValidator : AbstractValidator<UpdateMedicamentWarehouseCommand>
    {
        public UpdateMedicamentWarehouseCommandValidator()
        {
            RuleFor(x => x.ID_stan_leku).NotEmpty();

            RuleFor(x => x.request.Ilosc).NotEmpty().GreaterThan(0).LessThanOrEqualTo(9999);

            RuleFor(x => x.request.DataWaznosci).NotEmpty().GreaterThan(DateTime.Now);
        }
    }
}
