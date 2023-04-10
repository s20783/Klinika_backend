using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Commands
{
    public class DeleteReceptaCommand : IRequest<int>
    {
        public string ID_recepta { get; set; }
    }

    public class DeleteReceptaCommandHandler : IRequestHandler<DeleteReceptaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteReceptaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeleteReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_recepta);

            _context.ReceptaLeks.RemoveRange(_context.ReceptaLeks.Where(x => x.IdWizyta.Equals(id)).ToList());
            _context.Recepta.Remove(_context.Recepta.Where(x => x.IdWizyta.Equals(id)).First());
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}