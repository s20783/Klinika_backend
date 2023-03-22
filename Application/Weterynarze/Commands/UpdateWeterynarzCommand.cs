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

    public class UpdateWeterynarzCommandHandle : IRequestHandler<UpdateWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetWeterynarzListResponse> cache;
        public UpdateWeterynarzCommandHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetWeterynarzListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<int> Handle(UpdateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var konto = context.Osobas.Where(x => x.IdOsoba == id).First();
            var weterynarz = context.Weterynarzs.Where(x => x.IdOsoba == id).First();

            konto.Imie = req.request.Imie;
            konto.Nazwisko = req.request.Nazwisko;
            konto.Email = req.request.Email;
            konto.NumerTelefonu = req.request.NumerTelefonu;

            weterynarz.DataUrodzenia = req.request.DataUrodzenia.Date;
            weterynarz.Pensja = req.request.Pensja;
            weterynarz.DataZatrudnienia = req.request.DataZatrudnienia.Date;

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}