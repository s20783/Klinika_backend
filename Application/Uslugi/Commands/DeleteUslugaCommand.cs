using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Commands
{
    public class DeleteUslugaCommand : IRequest<int>
    {
        public string ID_usluga { get; set; }
    }

    public class DeleteUslugaCommandHandler : IRequestHandler<DeleteUslugaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetUslugaResponse> _cache;
        public DeleteUslugaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetUslugaResponse> cache)
        {
            _context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(DeleteUslugaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_usluga);

            _context.WizytaUslugas.RemoveRange(_context.WizytaUslugas.Where(x => x.IdUsluga.Equals(id)).ToList());
            _context.Uslugas.Remove(_context.Uslugas.Where(x => x.IdUsluga.Equals(id)).First());

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}