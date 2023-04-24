using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class VisitVetQuery : IRequest<List<GetVisitListResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class WizytaWeterynarzQueryHandler : IRequestHandler<VisitVetQuery, List<GetVisitListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public WizytaWeterynarzQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<List<GetVisitListResponse>> Handle(VisitVetQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_weterynarz);

            return (from x in _context.Wizyta
                    join d in _context.Harmonograms on x.IdWizyta equals d.IdWizyta into harmonogram
                    from y in harmonogram.DefaultIfEmpty()
                    join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                    join d in _context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                    from p in pacjent.DefaultIfEmpty()
                    where y.WeterynarzIdOsoba == id
                    group x by new { x.IdWizyta, x.IdOsoba, x.IdPacjent, x.Status, x.CzyOplacona, p.Nazwa, y.WeterynarzIdOsoba, k.Imie, k.Nazwisko }
                    into g
                    select new GetVisitListResponse()
                    {
                        IdWizyta = _hash.Encode(g.Key.IdWizyta),
                        IdPacjent = g.Key.IdPacjent != null ? _hash.Encode((int)g.Key.IdPacjent) : null,
                        Pacjent = g.Key.IdPacjent != null ? g.Key.Nazwa : null,
                        IdKlient = g.Key.IdOsoba != null ? _hash.Encode(g.Key.IdOsoba) : "",
                        Klient = g.Key.Imie + " " + g.Key.Nazwisko,
                        IdWeterynarz = req.ID_weterynarz,
                        Weterynarz = g.Key.WeterynarzIdOsoba != null ? _context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                        Status = g.Key.Status,
                        CzyOplacona = g.Key.CzyOplacona,
                        Data = g.Key.WeterynarzIdOsoba != null ? _context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).OrderBy(x => x.DataRozpoczecia).Select(x => x.DataRozpoczecia).First() : null
                    })
                    .AsParallel()
                    .WithCancellation(cancellationToken)
                    .ToList()
                    .OrderByDescending(x => x.Data)
                    .ToList();
        }
    }
}