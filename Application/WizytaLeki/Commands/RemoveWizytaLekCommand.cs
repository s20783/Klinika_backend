using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaLeki.Commands
{
    public class RemoveWizytaLekCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_lek { get; set; }
    }

    public class RemoveWizytaLekCommandHandler : IRequestHandler<RemoveWizytaLekCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public RemoveWizytaLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(RemoveWizytaLekCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = _hash.Decode(req.ID_wizyta, req.ID_lek);

            _context.WizytaLeks.Remove(_context.WizytaLeks.First(x => x.IdWizyta == id1 && x.IdLek == id2));

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}