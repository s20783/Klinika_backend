using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChorobaLeki.Commands
{
    public class RemoveChorobaLekCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public string ID_choroba { get; set; }
    }

    public class RemoveChorobaLekCommandHandler : IRequestHandler<RemoveChorobaLekCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public RemoveChorobaLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(RemoveChorobaLekCommand req, CancellationToken cancellationToken)
        {
            int lekID = _hash.Decode(req.ID_lek);
            int chorobaID = _hash.Decode(req.ID_choroba);

            var result = _context.ChorobaLeks.Where(x => x.IdLek.Equals(lekID) && x.IdChoroba.Equals(chorobaID)).First();
            _context.ChorobaLeks.Remove(result);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
