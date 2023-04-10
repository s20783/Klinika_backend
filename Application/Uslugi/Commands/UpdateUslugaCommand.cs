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

namespace Application.Uslugi.Commands
{
    public class UpdateUslugaCommand : IRequest<int>
    {
        public string ID_usluga { get; set; }
        public UslugaRequest request { get; set; }
    }

    public class UpdateUslugaCommandHandler : IRequestHandler<UpdateUslugaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetUslugaResponse> _cache;
        public UpdateUslugaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetUslugaResponse> cache)
        {
            _context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(UpdateUslugaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_usluga);
            
            var usluga = _context.Uslugas.Where(x => x.IdUsluga.Equals(id)).First();
            usluga.NazwaUslugi = req.request.NazwaUslugi;
            usluga.Opis = req.request.Opis;
            usluga.Cena = req.request.Cena;
            usluga.Dolegliwosc = req.request.Dolegliwosc;
            usluga.Narkoza = req.request.Narkoza;

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}