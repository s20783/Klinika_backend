using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Commands
{
    public class DeleteVaccinationCommand : IRequest<int>
    {
        public string ID_szczepienie { get; set; }
    }

    public class DeleteSzczepienieCommandHandler : IRequestHandler<DeleteVaccinationCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteSzczepienieCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeleteVaccinationCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_szczepienie);

            var szczepienie = _context.Szczepienies.Where(x => x.IdSzczepienie.Equals(id)).First();
            _context.Szczepienies.Remove(szczepienie);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}