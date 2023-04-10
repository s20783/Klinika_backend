using Application.DTO.Requests;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Commands
{
    public class UpdateChorobaCommand : IRequest<int>
    {
        public string ID_Choroba { get; set; }
        public ChorobaRequest request { get; set; }
    }

    public class UpdateChorobaCommandHandler : IRequestHandler<UpdateChorobaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetChorobaResponse> _cache;
        public UpdateChorobaCommandHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetChorobaResponse> cache)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
        }

        public async Task<int> Handle(UpdateChorobaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Choroba);
            var choroba = _context.Chorobas.Where(x => x.IdChoroba.Equals(id)).First();

            if (!choroba.Nazwa.Equals(req.request.Nazwa))
            {
                if (_context.Chorobas.Where(x => x.Nazwa.Equals(req.request.Nazwa)).Any())
                {
                    throw new Exception("already exists");
                }
            }

            choroba.Nazwa = req.request.Nazwa;
            choroba.NazwaLacinska = req.request.NazwaLacinska;
            choroba.Opis = req.request.Opis;

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}