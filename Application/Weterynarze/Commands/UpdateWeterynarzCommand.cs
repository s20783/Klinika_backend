using Application.DTO;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class UpdateWeterynarzCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public WeterynarzUpdateRequest request { get; set; }
    }

    public class UpdateWeterynarzCommandHandler : IRequestHandler<UpdateWeterynarzCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetWeterynarzListResponse> _cache;
        public UpdateWeterynarzCommandHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetWeterynarzListResponse> cache)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
        }

        public async Task<int> Handle(UpdateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            var konto = _context.Osobas.Where(x => x.IdOsoba == id).First();
            var weterynarz = _context.Weterynarzs.Where(x => x.IdOsoba == id).First();

            konto.Imie = req.request.Imie;
            konto.Nazwisko = req.request.Nazwisko;
            konto.Email = req.request.Email;
            konto.NumerTelefonu = req.request.NumerTelefonu;

            weterynarz.DataUrodzenia = req.request.DataUrodzenia.Date;
            weterynarz.Pensja = req.request.Pensja;
            weterynarz.DataZatrudnienia = req.request.DataZatrudnienia.Date;

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}