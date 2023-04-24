using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Commands
{
    public class DeleteMedicamentWarehouseCommandValidator : AbstractValidator<DeleteMedicamentWarehouseCommand>
    {
        public DeleteMedicamentWarehouseCommandValidator()
        {
            RuleFor(x => x.ID_stan_leku).NotEmpty();
        }
    }
}