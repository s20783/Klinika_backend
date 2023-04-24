using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Commands
{
    public class DeleteMedicamentWarehouseCommand : IRequest<int>
    {
        public string ID_stan_leku { get; set; }
    }

    public class DeleteStanLekuCommandHandler : IRequestHandler<DeleteMedicamentWarehouseCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteStanLekuCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeleteMedicamentWarehouseCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_stan_leku);

            _context.LekWMagazynies.Remove(_context.LekWMagazynies.Where(x => x.IdStanLeku == id).First());
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}