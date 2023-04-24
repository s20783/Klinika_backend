using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepionki.Commands
{
    public class DeleteVaccineCommand : IRequest<int>
    {
        public string ID_szczepionka { get; set; }
    }

    public class DeleteSzczepionkaCommandHandler : IRequestHandler<DeleteVaccineCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteSzczepionkaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeleteVaccineCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_szczepionka);

            _context.LekWMagazynies.RemoveRange(_context.LekWMagazynies.Where(x => x.IdLek.Equals(id)).ToList());
            _context.Szczepienies.RemoveRange(_context.Szczepienies.Where(x => x.IdLek.Equals(id)).ToList());
            _context.Szczepionkas.Remove(_context.Szczepionkas.Where(x => x.IdLek.Equals(id)).First());
            _context.Leks.Remove(_context.Leks.Where(x => x.IdLek.Equals(id)).First());

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}